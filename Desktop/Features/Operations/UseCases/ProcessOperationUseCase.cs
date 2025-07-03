using System;
using System.Threading;
using System.Threading.Tasks;
using Common.ResultOf;
using Desktop.Core.Interfaces;
using Desktop.Core.Types;
using Desktop.Features.Logfiles.Domain;
using Desktop.Features.Logfiles.UseCases;
using Desktop.Features.Operations.Domain;
using Desktop.Features.OperationTasks.Domain;

namespace Desktop.Features.Operations.UseCases;

public class ProcessOperationUseCase(
    AddOperationUseCase addOperationUseCase,
    LogfilesUseCases logfilesUseCases
) : IUseCase<ProcessOperationCommand, Operation>
{
    private readonly OperationsParserFactory _operationsParserFactory = new();
    private readonly OperationTaskParserFactory _operationTaskParserFactory = new();

    public async Task<ResultOf<Operation>> ExecuteAsync(ProcessOperationCommand command, CancellationToken ct = default)
    {
        var logfileResult = await this.ExecuteMoveLogfileToBackupUseCase(command, ct);
        if (logfileResult.IsFailure)
        {
            return ResultOf<Operation>.Failure(logfileResult.Errors);
        }

        logfileResult.SuccessValue.Type = command.LogfileType;

        var operationResult = this.ParseOperation(logfileResult.SuccessValue);
        if (operationResult.IsFailure)
        {
            return ResultOf<Operation>.Failure(operationResult.Errors);
        }

        var addLogfileToSfcResult = await this.ExecuteAddLogfileToSfcUseCase(logfileResult.SuccessValue, command, ct);
        this.AddManufacturingOperationTask(operationResult.SuccessValue, logfileResult.SuccessValue);
        this.AddSfcOperationTask(operationResult.SuccessValue, addLogfileToSfcResult);

        return await addOperationUseCase.ExecuteAsync(new AddOperationCommand(
            operationResult.SuccessValue), ct);
    }

    private Task<ResultOf<Logfile>> ExecuteMoveLogfileToBackupUseCase(
        ProcessOperationCommand command,
        CancellationToken ct)
    {
        return logfilesUseCases.MoveLogfileToBackup.ExecuteAsync(new MoveLogfileToBackupCommand(
            Logfile: command.InputLogfile,
            DestinationDirectory: command.BackupDirectory
        ), ct);
    }

    private ResultOf<Operation> ParseOperation(Logfile logfile)
    {
        try
        {
            return _operationsParserFactory
                .Create(logfile.Type)
                .Parse(logfile);
        }
        catch (Exception e)
        {
            return ResultOf<Operation>.Failure(e.Message);
        }
    }

    private void AddManufacturingOperationTask(
        Operation operation,
        Logfile logfile)
    {
        var operationTask = _operationTaskParserFactory
            .Create(OperationTaskType.Manufacturing, logfile.Type)
            .Parse(logfile);
        operationTask.Logfile = logfile;
        operation.Tasks.Add(operationTask);
    }

    private async Task<ResultOf<Logfile>> ExecuteAddLogfileToSfcUseCase(
        Logfile logfile,
        ProcessOperationCommand command,
        CancellationToken ct)
    {
        return await logfilesUseCases.AddLogfileToSfcUseCase.ExecuteAsync(
            new AddLogfileToSfcCommand(
                LogfileToUpload: logfile,
                OkResponses: command.OkResponses,
                Timeout: command.Timeout,
                BackupDirectory: command.BackupDirectory,
                MaxRetries: command.MaxRetries),
            ct);
    }

    private void AddSfcOperationTask(
        Operation operation,
        ResultOf<Logfile> result,
        string okResponses = "OK")
    {
        var sfcResponseLogfile = result.IsSuccess
            ? result.SuccessValue
            : null;

        var operationTask = _operationTaskParserFactory
            .CreateSfcResponseOperationTaskParser(sfcResponseLogfile?.Type ?? LogfileType.SfcDefault)
            .SetOkResponses(okResponses)
            .SetErrors(result.Errors)
            .Parse(sfcResponseLogfile);

        operation.Tasks.Add(operationTask);
    }
}
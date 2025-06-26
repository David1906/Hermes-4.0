using Common.ResultOf;
using Domain.Core.Types;
using Domain.Logfiles;
using Domain.Operations;
using System.Collections.Immutable;
using UseCases.Logfiles;

namespace UseCases.Operations;

public class ProcessOperation(
    AddOperation addOperation,
    LogfilesUseCases logfilesUseCases,
    TriOperationParser triOperationParser
) : IUseCase<ProcessOperationCommand, Operation>
{
    public async Task<ResultOf<Operation>> ExecuteAsync(ProcessOperationCommand command, CancellationToken ct = default)
    {
        return await this.AddLogfileToSfc(command, ct)
            .Combine((_, _) => this.MoveLogfileToBackup(command, ct), ct)
            .Map(this.AddOperation, ct)
            .Tap(x => this.FinallyAddOperation(x, command.InputLogfile));
    }

    private async Task<ResultOf<(Logfile, Logfile)>> FinallyAddOperation(
        ResultOf<Operation> result,
        Logfile inputLogfile)
    {
        var operation = result.Match(
            x => this.Parse(x),
            x => this.Parse(inputLogfile)
        );
        await addOperation.ExecuteAsync(new AddOperationCommand(
            operation
        ));
        return result;
    }

    private Operation Parse(
        ( Logfile response, Logfile input) logfiles,
        ImmutableArray<Error>? errors = null)
    {
        var operation = triOperationParser.Parse(logfiles.input);
        operation.UploadLogfile = logfiles.response;
        if (errors != null)
        {
            operation.UploadResult = UploadResultType.Ok;
        }
        else
        {
            operation.UploadResult = UploadResultType.Fail;
        }

        return operation;
    }

    private async Task<ResultOf<Logfile>> AddLogfileToSfc(ProcessOperationCommand command, CancellationToken ct)
    {
        var response = await logfilesUseCases.AddLogfileToSfc.ExecuteAsync(
            new AddLogfileToSfcCommand(
                LogfileToUpload: command.InputLogfile,
                OkResponses: command.OkResponses,
                Timeout: command.Timeout,
                BackupDirectory: command.BackupDirectory,
                MaxRetries: command.MaxRetries),
            ct);
        return response;
    }

    private async Task<ResultOf<Logfile>> MoveLogfileToBackup(ProcessOperationCommand command, CancellationToken ct)
    {
        return await logfilesUseCases.MoveLogfileToBackup.ExecuteAsync(new MoveLogfileToBackupCommand(
            Logfile: command.InputLogfile,
            DestinationDirectory: command.BackupDirectory
        ), ct);
    }

    private async Task<Operation> AddOperation(
        ( Logfile response, Logfile input) logfiles,
        CancellationToken ct)
    {
        var operation = triOperationParser.Parse(logfiles.input);
        operation.UploadLogfile = logfiles.response;
        operation.UploadResult = UploadResultType.Ok;
        return await addOperation.ExecuteAsync(new AddOperationCommand(
                operation
            ), ct)
            .Match(
                _ => Task.FromResult(operation),
                _ => Task.FromResult(operation));
    }
}
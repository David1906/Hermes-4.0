using Common.ResultOf;
using Common;
using Core.Application.Common.Data;
using Core.Application.Common.Events;
using Core.Application.Common.FileParsers;
using Core.Application.Common.Gateways;
using Core.Application.Common.Types;
using Core.Application.Features.Logfiles.Commands;
using Core.Domain;
using Paramore.Brighter;

namespace Core.Application.Features.Panels.Commands;

public class SendPanelToNextStationHandler(
    IUnitOfWork uow,
    IAmACommandProcessor commandProcessor,
    IResilientFileSystem fileSystem,
    ILogfilesSfcGateway logfilesSfcGateway,
    IOperationParser operationParser
) : RequestHandlerAsync<SendPanelToNextStationCommand>
{
    public override async Task<SendPanelToNextStationCommand> HandleAsync(
        SendPanelToNextStationCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!this.EnsureSfcPathCommunication())
        {
            // TODO: Show stop based on failure for EE/IT
            command.Result = ResultOf<Panel>.Failure(Error.ConnectionError);
            return command;
        }

        var operation = await this.SendPanelToNextStation(command, cancellationToken);
        if (operation.IsFailure)
        {
            await commandProcessor.SendAsync(new ShowStopEvent()
            {
                Title = operation.Logfile is not null
                    ? await fileSystem.ReadAllTextAsync(operation.Logfile.FullName, cancellationToken)
                    : operation.Result.ToString(),
                SerialNumber = command.Panel.MainSerialNumber,
                Departments = [DepartmentType.EE]
            }, cancellationToken: cancellationToken);
            command.Result = ResultOf<Panel>.Failure(Error.ConnectionError);
            return command;
        }

        await commandProcessor.SendAsync(new ShowSuccessEvent()
        {
            Title = "OK",
            SerialNumber = command.Panel.MainSerialNumber,
            IsRepair = command.Panel.ContainsFailedBoard
        }, cancellationToken: cancellationToken);

        await uow.SaveChangesAsync(cancellationToken);
        command.Result = command.Panel;
        return await base.HandleAsync(command, cancellationToken);
    }

    private bool EnsureSfcPathCommunication()
    {
        // TODO
        return true;
    }

    private async Task<Operation> SendPanelToNextStation(
        SendPanelToNextStationCommand command,
        CancellationToken ct)
    {
        var sendResult = await logfilesSfcGateway.SendPanelToNextStationAsync(
                command.InputLogfile,
                command.MaxRetries,
                command.Timeout,
                ct)
            .Bind((x, _) => MoveLogfileToBackupAsync(x, command.BackupDirectory, ct), ct: ct);

        return await operationParser.ParseSfcResponseAsync(
            sendResult.IsSuccess ? sendResult.SuccessValue : null,
            sendResult.Errors,
            command.OkResponses);
    }

    private async Task<ResultOf<Logfile>> MoveLogfileToBackupAsync(
        Logfile logfile,
        DirectoryInfo backupDirectory,
        CancellationToken ct)
    {
        var moveLogfileToBackupCommand = new MoveLogfileToBackupCommand
        {
            Logfile = logfile,
            DestinationDirectory = backupDirectory
        };
        await commandProcessor.SendAsync(moveLogfileToBackupCommand, cancellationToken: ct);
        return moveLogfileToBackupCommand.Result;
    }
}
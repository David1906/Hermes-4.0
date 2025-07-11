using Common.ResultOf;
using Core.Application.Common.Errors;
using Core.Application.Common.Events;
using Core.Application.Common.FileParsers;
using Core.Application.Common.Gateways;
using Core.Application.Features.Logfiles.Commands;
using Core.Domain;
using Core.Domain.Common.Types;
using Paramore.Brighter;

namespace Core.Application.Features.Panels.Commands;

public class SendPanelToNextStationHandler(
    IAmACommandProcessor commandProcessor,
    ISfcSharedFolderGateway sfcSharedFolderGateway,
    ISfcResponseOperationParser sfcResponseOperationParser
) : RequestHandlerAsync<SendPanelToNextStationCommand>
{
    public override async Task<SendPanelToNextStationCommand> HandleAsync(
        SendPanelToNextStationCommand command,
        CancellationToken cancellationToken = default)
    {
        var operation = command.ResultOperation;
        command.Panel.Operations.Add(operation);
        operation.Start();

        if (!this.EnsureSfcPathCommunication())
        {
            operation.End(new ConnectionError());
            await SendShowStopEventAsync(command, cancellationToken, operation);
            return await base.HandleAsync(command, cancellationToken);
        }

        await this.SendPanelToNextStation(operation, command, cancellationToken);
        if (operation.IsFailure)
        {
            await SendShowStopEventAsync(command, cancellationToken, operation);
            return await base.HandleAsync(command, cancellationToken);
        }

        await SendSuccessEventAsync(command, cancellationToken);
        return await base.HandleAsync(command, cancellationToken);
    }

    private async Task SendSuccessEventAsync(SendPanelToNextStationCommand command, CancellationToken cancellationToken)
    {
        await commandProcessor.SendAsync(new ShowSuccessEvent()
        {
            Title = "OK",
            MainSerialNumber = command.Panel.MainSerialNumber,
            IsRepair = command.Panel.ContainsFailedBoard
        }, cancellationToken: cancellationToken);
    }

    private async Task SendShowStopEventAsync(SendPanelToNextStationCommand command,
        CancellationToken cancellationToken,
        Operation operation)
    {
        await commandProcessor.SendAsync(new ShowStopEvent()
        {
            Title = operation.Title,
            ErrorType = operation.TranslatedErrorType,
            SerialNumber = command.Panel.MainSerialNumber,
            Departments = [DepartmentType.EE]
        }, cancellationToken: cancellationToken);
    }

    private bool EnsureSfcPathCommunication()
    {
        // TODO
        return true;
    }

    private async Task SendPanelToNextStation(
        Operation operation,
        SendPanelToNextStationCommand command,
        CancellationToken ct)
    {
        await sfcSharedFolderGateway.SendPanelToNextStationAsync(
                command.InputLogfile,
                command.MaxRetries,
                command.Timeout,
                ct)
            .Bind((x, _) => MoveLogfileToBackupAsync(x, command.BackupDirectory, ct), ct: ct)
            .OnSuccess(async logfile =>
            {
                operation.Logfile = logfile;
                operation.Error = await sfcResponseOperationParser.ParseErrorAsync(logfile, command.OkResponses);
                operation.End();
            })
            .OnFailure(operation.End);
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
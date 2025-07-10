using Common.Extensions;
using Common.ResultOf;
using Core.Application.Common.Data;
using Core.Application.Common.Extensions;
using Core.Application.Features.Logfiles.Commands;
using Core.Domain;
using Paramore.Brighter;

namespace Core.Application.Features.Panels.Commands;

public class ProcessPanelFromLogfileHandler(
    IUnitOfWork uow,
    IAmACommandProcessor commandProcessor
) : RequestHandlerAsync<ProcessPanelFromLogfileCommand>
{
    public override async Task<ProcessPanelFromLogfileCommand> HandleAsync(
        ProcessPanelFromLogfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var logfileResult = await this.MoveLogfileToBackupAsync(
            command.InputLogfile, command.BackupDirectory, cancellationToken);
        if (logfileResult.IsFailure)
        {
            command.Result = ResultOf<Panel>.Failure(logfileResult.Errors);
            return command;
        }

        var panelResult = await this.CreatePanelFromMachineLogfileAsync(command, cancellationToken);
        if (panelResult.IsFailure)
        {
            commandProcessor.ShowErrorToast(panelResult.Errors.JoinWithNewLine());
            command.Result = panelResult;
            return command;
        }

        command.Result = await this.SendPanelToNextStation(panelResult.SuccessValue, command, cancellationToken);

        await uow.SaveChangesAsync(cancellationToken);
        return await base.HandleAsync(command, cancellationToken);
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

    private async Task<ResultOf<Panel>> CreatePanelFromMachineLogfileAsync(
        ProcessPanelFromLogfileCommand toNextStationCommand,
        CancellationToken ct)
    {
        var createPanelCommand = new CreatePanelFromLogfileCommand()
        {
            Logfile = toNextStationCommand.InputLogfile
        };
        await commandProcessor.SendAsync(createPanelCommand, cancellationToken: ct);
        return createPanelCommand.Result;
    }

    private async Task<ResultOf<Panel>> SendPanelToNextStation(
        Panel panel,
        ProcessPanelFromLogfileCommand command,
        CancellationToken ct)
    {
        var nextStationCommand = new SendPanelToNextStationCommand()
        {
            Panel = panel,
            InputLogfile = command.InputLogfile,
            BackupDirectory = command.BackupDirectory,
            OkResponses = command.OkResponses,
            Timeout = command.Timeout,
            MaxRetries = command.MaxRetries
        };
        await commandProcessor.SendAsync(nextStationCommand, cancellationToken: ct);
        return nextStationCommand.Result;
    }
}
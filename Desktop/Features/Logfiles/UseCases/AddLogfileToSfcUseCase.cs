using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common.ResultOf;
using Core.Application.Common.Errors;
using Desktop.Core.Interfaces;
using Desktop.Features.Logfiles.Domain;

namespace Desktop.Features.Logfiles.UseCases;

public class AddLogfileToSfcUseCase(
    ILogfilesSfcGateway logfilesSfcGateway,
    MoveLogfileToBackup moveLogfileToBackup
) : IUseCase<AddLogfileToSfcCommand, Logfile>
{
    public async Task<ResultOf<Logfile>> ExecuteAsync(
        AddLogfileToSfcCommand command,
        CancellationToken ct = default)
    {
        if (!this.EnsureSfcPathCommunication())
        {
            return ResultOf<Logfile>.Failure(new ConnectionError());
        }

        return await logfilesSfcGateway.UploadOperationAsync(
                command.LogfileToUpload,
                command.MaxRetries,
                command.Timeout,
                ct)
            .Bind((responseLogfile, _) => MoveLogfileToBackup(responseLogfile, command.BackupDirectory, ct), ct);
    }

    private async Task<ResultOf<Logfile>> MoveLogfileToBackup(
        Logfile logfile,
        DirectoryInfo backupDirectory,
        CancellationToken ct)
    {
        return await moveLogfileToBackup.ExecuteAsync(
            new MoveLogfileToBackupCommand(
                Logfile: logfile,
                DestinationDirectory: backupDirectory),
            ct);
    }

    private bool EnsureSfcPathCommunication()
    {
        // TODO
        return true;
    }
}
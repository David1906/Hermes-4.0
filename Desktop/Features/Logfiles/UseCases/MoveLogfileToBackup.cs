using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.ResultOf;
using Desktop.Core.Interfaces;
using Desktop.Features.Logfiles.Domain;

namespace Desktop.Features.Logfiles.UseCases;

public class MoveLogfileToBackup(IResilientFileSystem resilientFileSystem)
    : IUseCase<MoveLogfileToBackupCommand, Logfile>
{
    public async Task<ResultOf<Logfile>> ExecuteAsync(MoveLogfileToBackupCommand command,
        CancellationToken ct = default)
    {
        try
        {
            var destinationFullPath = Path.Combine(
                command.DestinationDirectory.FullName,
                $"{DateTime.Now:yyyyMMdd}",
                command.Logfile.Name);
            await resilientFileSystem.MoveFileAsync(command.Logfile.FullName, destinationFullPath, ct);
            return new Logfile
            {
                FileInfo = new FileInfo(destinationFullPath),
                Content = command.Logfile.Content
            };
        }
        catch (Exception e)
        {
            return ResultOf<Logfile>.Failure(e.Message);
        }
    }
}
using Common;
using Domain.Core.Errors;
using Domain.Logfiles;
using OneOf;

namespace UseCases.Logfiles;

public class MoveLogfileToBackup(IResilientFileSystem resilientFileSystem)
    : IUseCase<MoveLogfileToBackupCommand, Logfile>
{
    public async Task<OneOf<Logfile, Error>> ExecuteAsync(MoveLogfileToBackupCommand command,
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
            return new Error(e.Message);
        }
    }
}
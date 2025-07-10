using Common.ResultOf;
using Common;
using Core.Domain;
using Paramore.Brighter;

namespace Core.Application.Features.Logfiles.Commands;

public class MoveLogfileToBackupHandler(IResilientFileSystem resilientFileSystem)
    : RequestHandlerAsync<MoveLogfileToBackupCommand>
{
    public override async Task<MoveLogfileToBackupCommand> HandleAsync(
        MoveLogfileToBackupCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var destinationFullPath = Path.Combine(
                command.DestinationDirectory.FullName,
                $"{DateTime.Now:yyyyMMdd}",
                command.Logfile.Name);
            await resilientFileSystem.MoveFileAsync(command.Logfile.FullName, destinationFullPath, cancellationToken);
            command.Logfile.FileInfo = new FileInfo(destinationFullPath);
            command.Result = ResultOf<Logfile>.Success(command.Logfile);
        }
        catch (Exception e)
        {
            command.Result = ResultOf<Logfile>.Failure(e.Message);
        }

        return await base.HandleAsync(command, cancellationToken);
    }
}
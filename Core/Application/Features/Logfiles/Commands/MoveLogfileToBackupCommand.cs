using Common.ResultOf;
using Core.Domain;
using Paramore.Brighter;

namespace Core.Application.Features.Logfiles.Commands;

public class MoveLogfileToBackupCommand() : Command(Guid.NewGuid())
{
    public required Logfile Logfile { get; init; }
    public required DirectoryInfo DestinationDirectory { get; init; }
    public ResultOf<Logfile> Result { get; set; } = ResultOf<Logfile>.Failure(Error.Unknown);
}
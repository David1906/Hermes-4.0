using Common.ResultOf;
using Core.Domain;
using Paramore.Brighter;

namespace Core.Application.Features.Panels.Commands;

public class ProcessPanelFromLogfileCommand() : Command(Guid.NewGuid())
{
    public required Logfile InputLogfile { get; init; }
    public required DirectoryInfo BackupDirectory { get; init; }
    public required string OkResponses { get; init; }
    public required TimeSpan Timeout { get; init; }
    public int MaxRetries { get; init; }
    public ResultOf<Panel> Result { get; set; } = ResultOf<Panel>.Failure(new UnknownError());
};
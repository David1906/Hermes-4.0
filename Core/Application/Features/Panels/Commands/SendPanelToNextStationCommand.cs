using Common.ResultOf;
using Core.Domain;
using Core.Domain.Common.Types;
using Paramore.Brighter;

namespace Core.Application.Features.Panels.Commands;

public class SendPanelToNextStationCommand() : Command(Guid.NewGuid())
{
    public required Panel Panel { get; init; }
    public required Logfile InputLogfile { get; init; }
    public required DirectoryInfo BackupDirectory { get; init; }
    public required string OkResponses { get; init; }
    public required TimeSpan Timeout { get; init; }
    public int MaxRetries { get; init; }
    public Operation ResultOperation { get; } = new() { Type = OperationType.SendPanelToNextStation };
};
using Common.ResultOf;
using Core.Domain;
using Paramore.Brighter;

namespace Core.Application.Features.Panels.Commands;

public class CreatePanelFromLogfileCommand() : Command(Guid.NewGuid())
{
    public required Logfile Logfile { get; init; }
    public ResultOf<Panel> Result { get; set; } = ResultOf<Panel>.Failure(Error.Unknown);
}
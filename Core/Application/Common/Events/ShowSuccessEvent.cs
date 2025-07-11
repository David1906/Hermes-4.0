using Paramore.Brighter;

namespace Core.Application.Common.Events;

public class ShowSuccessEvent() : Event(Guid.NewGuid())
{
    public string Title { get; set; } = "OK";
    public string Message { get; set; } = "";
    public string MainSerialNumber { get; set; } = "";
    public bool IsRepair { get; set; }
}
using Core.Domain.Common.Types;
using Paramore.Brighter;

namespace Core.Application.Common.Events;

public class ShowToastEvent() : Event(Guid.NewGuid())
{
    public const string DefaultChannel = "default";
    public string Channel { get; set; } = DefaultChannel;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType ToastType { get; set; } = NotificationType.Information;
    public bool AutoDismiss { get; set; } = true;
    public TimeSpan AutoDismissDelay { get; set; } = TimeSpan.FromSeconds(5);
}
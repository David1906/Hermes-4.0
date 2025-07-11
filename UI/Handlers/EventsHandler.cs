using Core.Application.Common.Events;
using R3;

namespace UI.Handlers;

public class EventsHandler
{
    public Subject<OperationCreatedEvent> OperationCreated { get; } = new();
    public Subject<ShowToastEvent> ShowToast { get; } = new();
}
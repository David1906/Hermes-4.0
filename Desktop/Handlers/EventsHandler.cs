using Desktop.Events;
using R3;
using UseCases.Operations;
using UseCases.OperationTasks;

namespace Desktop.Handlers;

public class EventsHandler
{
    public Subject<OperationCreatedEvent> OperationCreated { get; } = new();
    public Subject<OperationTaskCreatedEvent> OperationTaskCreated { get; } = new();
    public Subject<ShowToastEvent> ShowToast { get; } = new();
}
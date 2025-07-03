using Desktop.Common.Events;
using Desktop.Features.Operations.UseCases;
using Desktop.Features.OperationTasks.UseCases;
using R3;

namespace Desktop.Handlers;

public class EventsHandler
{
    public Subject<OperationCreatedEvent> OperationCreated { get; } = new();
    public Subject<OperationTaskCreatedEvent> OperationTaskCreated { get; } = new();
    public Subject<ShowToastEvent> ShowToast { get; } = new();
}
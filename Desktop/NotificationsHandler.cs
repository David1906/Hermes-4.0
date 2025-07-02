using R3;
using UseCases.Operations;
using UseCases.OperationTasks;

namespace Desktop;

public class NotificationsHandler
{
    public Subject<OperationCreatedEvent> OperationCreated { get; } = new();
    public Subject<OperationTaskCreatedEvent> OperationTaskCreated { get; } = new();
}
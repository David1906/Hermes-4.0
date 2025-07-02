using Paramore.Brighter;
using System.Threading.Tasks;
using System.Threading;
using UseCases.OperationTasks;

namespace Desktop;

public class OperationTaskCreatedHandler(NotificationsHandler notificationsHandler)
    : RequestHandlerAsync<OperationTaskCreatedEvent>
{
    public override Task<OperationTaskCreatedEvent> HandleAsync(
        OperationTaskCreatedEvent @event,
        CancellationToken ct = default)
    {
        notificationsHandler.OperationTaskCreated.OnNext(@event);
        return base.HandleAsync(@event, ct);
    }
}
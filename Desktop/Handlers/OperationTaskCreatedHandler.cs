using System.Threading;
using System.Threading.Tasks;
using Paramore.Brighter;
using UseCases.OperationTasks;

namespace Desktop.Handlers;

public class OperationTaskCreatedHandler(EventsHandler eventsHandler)
    : RequestHandlerAsync<OperationTaskCreatedEvent>
{
    public override Task<OperationTaskCreatedEvent> HandleAsync(
        OperationTaskCreatedEvent @event,
        CancellationToken ct = default)
    {
        eventsHandler.OperationTaskCreated.OnNext(@event);
        return base.HandleAsync(@event, ct);
    }
}
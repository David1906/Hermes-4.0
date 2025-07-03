using System.Threading;
using System.Threading.Tasks;
using Desktop.Features.Operations.UseCases;
using Paramore.Brighter;

namespace Desktop.Handlers;

public class OperationCreatedHandler(EventsHandler eventsHandler)
    : RequestHandlerAsync<OperationCreatedEvent>
{
    public override async Task<OperationCreatedEvent> HandleAsync(
        OperationCreatedEvent @event,
        CancellationToken ct = default)
    {
        eventsHandler.OperationCreated.OnNext(@event);
        return await base.HandleAsync(@event, ct);
    }
}
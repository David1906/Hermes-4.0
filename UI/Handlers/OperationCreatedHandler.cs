using Core.Application.Common.Events;
using Paramore.Brighter;
using System.Threading.Tasks;
using System.Threading;

namespace UI.Handlers;

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
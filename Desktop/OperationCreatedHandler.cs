using Paramore.Brighter;
using System.Threading.Tasks;
using System.Threading;
using System;
using UseCases.Operations;

namespace Desktop;

public class OperationCreatedHandler(NotificationsHandler notificationsHandler)
    : RequestHandlerAsync<OperationCreatedEvent>
{
    public override async Task<OperationCreatedEvent> HandleAsync(
        OperationCreatedEvent @event,
        CancellationToken ct = default)
    {
        notificationsHandler.OperationCreated.OnNext(@event);
        return await base.HandleAsync(@event, ct);
    }
}
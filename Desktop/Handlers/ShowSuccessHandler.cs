using System.Threading;
using System.Threading.Tasks;
using Common.ResultOf;
using Core.Application.Common.Events;
using Desktop.Common;
using Desktop.Common.Events;
using Desktop.Features.Operations.Delivery;
using Paramore.Brighter;

namespace Desktop.Handlers;

public class ShowSuccessHandler(
    IAmACommandProcessor commandProcessor,
    ViewModelFactory viewModelFactory) : RequestHandlerAsync<ShowSuccessEvent>
{
    public override async Task<ShowSuccessEvent> HandleAsync(
        ShowSuccessEvent @event,
        CancellationToken cancellationToken = default)
    {
        await viewModelFactory.CreateSuccess(@event.SerialNumber)
            .OnSuccess(vm => commandProcessor.Publish(new OpenWindowEvent { ViewModel = vm }));
        return await base.HandleAsync(@event, cancellationToken);
    }
}
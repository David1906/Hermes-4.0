using Core.Application.Common.Events;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter;
using System.Threading.Tasks;
using System.Threading;
using System;
using UI.Common.Events;
using UI.Features.Operations;
using UI.Features.Panels;

namespace UI.Handlers;

public class ShowSuccessHandler(
    IAmACommandProcessor commandProcessor,
    IServiceProvider serviceProvider) : RequestHandlerAsync<ShowSuccessEvent>
{
    public override async Task<ShowSuccessEvent> HandleAsync(
        ShowSuccessEvent @event,
        CancellationToken cancellationToken = default)
    {
        var vm = serviceProvider.GetRequiredService<SuccessViewModel>();
        vm.SerialNumber = @event.MainSerialNumber;
        commandProcessor.Publish(new OpenWindowEvent { ViewModel = vm });
        return await base.HandleAsync(@event, cancellationToken);
    }
}
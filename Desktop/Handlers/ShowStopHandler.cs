using Core.Application.Common.Events;
using Desktop.Common.Events;
using Desktop.Features.Stops.Delivery;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Desktop.Handlers;

public class ShowStopHandler(
    IServiceProvider serviceProvider,
    IAmACommandProcessor commandProcessor) : RequestHandlerAsync<ShowStopEvent>
{
    public override async Task<ShowStopEvent> HandleAsync(
        ShowStopEvent @event,
        CancellationToken cancellationToken = default)
    {
        var stopViewModel = serviceProvider.GetRequiredService<StopViewModel>();
        stopViewModel.SerialNumber = @event.SerialNumber;
        stopViewModel.Title = @event.Title;
        stopViewModel.ErrorType = @event.ErrorType;
        stopViewModel.Departments = @event.Departments;
        commandProcessor.Publish(new OpenWindowEvent { ViewModel = stopViewModel });
        return await base.HandleAsync(@event, cancellationToken);
    }
}
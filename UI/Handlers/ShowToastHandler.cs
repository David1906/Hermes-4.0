using Core.Application.Common.Events;
using Paramore.Brighter;

namespace UI.Handlers;

public class ShowToastHandler(EventsHandler eventsHandler) : RequestHandler<ShowToastEvent>
{
    public override ShowToastEvent Handle(ShowToastEvent @event)
    {
        eventsHandler.ShowToast.OnNext(@event);
        return base.Handle(@event);
    }
}
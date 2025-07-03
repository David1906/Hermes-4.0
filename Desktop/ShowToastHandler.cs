using Desktop.Events;
using Desktop.Handlers;
using Paramore.Brighter;

namespace Desktop;

public class ShowToastHandler(EventsHandler eventsHandler) : RequestHandler<ShowToastEvent>
{
    public override ShowToastEvent Handle(ShowToastEvent @event)
    {
        eventsHandler.ShowToast.OnNext(@event);
        return base.Handle(@event);
    }
}
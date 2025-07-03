using Desktop.Common.Events;
using Paramore.Brighter;

namespace Desktop.Handlers;

public class ShowToastHandler(EventsHandler eventsHandler) : RequestHandler<ShowToastEvent>
{
    public override ShowToastEvent Handle(ShowToastEvent @event)
    {
        eventsHandler.ShowToast.OnNext(@event);
        return base.Handle(@event);
    }
}
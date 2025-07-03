using Avalonia.Controls;
using Desktop.Common.Events;
using Desktop.Core;
using Paramore.Brighter;

namespace Desktop.Handlers;

public class OpenWindowHandler : RequestHandler<OpenWindowEvent>
{
    private readonly ViewLocator _viewLocator = new();

    public override OpenWindowEvent Handle(OpenWindowEvent @event)
    {
        if (this._viewLocator.Build(@event.ViewModel) is not Window window)
        {
            return base.Handle(@event);
        }

        window.Show();
        window.Activate();
        return base.Handle(@event);
    }
}
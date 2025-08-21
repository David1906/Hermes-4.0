using Core.Application.Common.Events;
using Core.Domain.Common.Types;
using Infrastructure.Data;
using R3;
using SukiUI.Toasts;
using UI.Common;
using UI.Handlers;
using PanelProcessorViewModel = UI.Features.Panels.PanelProcessorViewModel;

namespace UI.Features.Main;

public class MainWindowViewModel : ViewModelBase
{
    private readonly EventsHandler _eventsHandler;

    public MainWindowViewModel(
        EventsHandler eventsHandler,
        HermesContext hermesContext,
        PanelProcessorViewModel panelProcessorViewModel)
    {
        _eventsHandler = eventsHandler;
        PanelProcessorViewModel = panelProcessorViewModel;

        hermesContext.Migrate();
        SetupRx();
    }

    public PanelProcessorViewModel PanelProcessorViewModel { get; }
    public ISukiToastManager ToastManager { get; } = new SukiToastManager();
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public BindableReactiveProperty<StateType> MachineState { get; } = new(StateType.Stopped);
    public BindableReactiveProperty<bool> IsConnected { get; } = new(false);

    private void SetupRx()
    {
        _eventsHandler.ShowToast
            .Subscribe(ShowToast)
            .AddTo(ref Disposables);
    }

    private void ShowToast(string message, NotificationType notificationType)
    {
        ShowToast(new ShowToastEvent
        {
            Message = message,
            ToastType = notificationType
        });
    }

    private void ShowToast(ShowToastEvent @event)
    {
        var toastBuilder = ToastManager.CreateToast()
            .WithTitle(@event.Title)
            .WithContent(@event.Message)
            .OfType((Avalonia.Controls.Notifications.NotificationType)@event.ToastType)
            .Dismiss().ByClicking();

        if (@event.ToastType != NotificationType.Error || @event.AutoDismiss)
            toastBuilder.Dismiss().After(@event.AutoDismissDelay);

        toastBuilder.Queue();
    }
}
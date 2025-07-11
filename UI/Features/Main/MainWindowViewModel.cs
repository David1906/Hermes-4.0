using Core.Application.Common.Events;
using Core.Domain.Common.Types;
using Infrastructure.Data;
using R3;
using SukiUI.Toasts;
using UI.Common;
using UI.Features.Operations;
using UI.Handlers;

namespace UI.Features.Main;

public partial class MainWindowViewModel : ViewModelBase
{
    public PanelProcessorViewModel PanelProcessorViewModel { get; }
    public ISukiToastManager ToastManager { get; } = new SukiToastManager();
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public BindableReactiveProperty<StateType> MachineState { get; } = new(StateType.Stopped);
    public BindableReactiveProperty<bool> IsConnected { get; } = new(false);

    private readonly EventsHandler _eventsHandler;

    public MainWindowViewModel(
        EventsHandler eventsHandler,
        HermesContext hermesContext,
        PanelProcessorViewModel panelProcessorViewModel)
    {
        this._eventsHandler = eventsHandler;
        this.PanelProcessorViewModel = panelProcessorViewModel;

        hermesContext.Migrate();
        this.SetupRx();
    }

    private void SetupRx()
    {
        _eventsHandler.ShowToast
            .Subscribe(this.ShowToast)
            .AddTo(ref Disposables);
    }

    private void ShowToast(string message, NotificationType notificationType)
    {
        this.ShowToast(new ShowToastEvent()
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
        {
            toastBuilder.Dismiss().After(@event.AutoDismissDelay);
        }

        toastBuilder.Queue();
    }
}
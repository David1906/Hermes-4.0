using Avalonia.Controls.Notifications;
using Desktop.Common;
using Desktop.Common.Events;
using Desktop.Core.Types;
using Desktop.Core;
using Desktop.Data;
using Desktop.Features.Operations.Delivery;
using Desktop.Handlers;
using R3;
using SukiUI.Toasts;

namespace Desktop.Features.Main.Delivery;

public partial class MainWindowViewModel : ViewModelBase
{
    public OperationProcessorViewModel OperationProcessorViewModel { get; }
    public ISukiToastManager ToastManager { get; } = new SukiToastManager();
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public BindableReactiveProperty<StateType> MachineState { get; } = new(StateType.Stopped);
    public BindableReactiveProperty<bool> IsConnected { get; } = new(false);

    private readonly EventsHandler _eventsHandler;

    public MainWindowViewModel(
        EventsHandler eventsHandler,
        SqliteContext sqliteContext,
        OperationProcessorViewModel operationProcessorViewModel)
    {
        this._eventsHandler = eventsHandler;
        this.OperationProcessorViewModel = operationProcessorViewModel;

        sqliteContext.Migrate();
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
            .OfType(@event.ToastType)
            .Dismiss().ByClicking();

        if (@event.ToastType != NotificationType.Error || @event.AutoDismiss)
        {
            toastBuilder.Dismiss().After(@event.AutoDismissDelay);
        }

        toastBuilder.Queue();
    }
}
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using Desktop.Core;
using Desktop.Events;
using Desktop.Features.Operations;
using Desktop.Handlers;
using Domain.Core.Types;
using Domain.Users;
using Infrastructure.Data;
using R3;
using SukiUI.Toasts;
using UseCases.Users;

namespace Desktop.Features.Main;

public partial class MainWindowViewModel : ViewModelBase
{
    public OperationProcessorViewModel OperationProcessorViewModel { get; }
    public ISukiToastManager ToastManager { get; } = new SukiToastManager();
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public BindableReactiveProperty<StateType> MachineState { get; } = new(StateType.Stopped);
    public BindableReactiveProperty<bool> IsConnected { get; } = new(false);

    private readonly UserUseCases _userUseCases;
    private readonly EventsHandler _eventsHandler;

    public MainWindowViewModel(
        EventsHandler eventsHandler,
        UserUseCases userUseCases,
        SqliteContext sqliteContext,
        OperationProcessorViewModel operationProcessorViewModel)
    {
        this._userUseCases = userUseCases;
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

    [RelayCommand]
    private async Task AddUserAsync()
    {
        var result = await _userUseCases.AddUser.ExecuteAsync(new AddUserRequest(
            Email,
            Name,
            Age
        ));

        result.Switch(
            userDto => this.ShowToast($"User {userDto.Name} added successfully.", NotificationType.Success),
            errors => this.ShowToast(errors.JoinWithNewLine(), NotificationType.Error)
        );
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
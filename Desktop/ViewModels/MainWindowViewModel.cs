using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.Input;
using Domain.Users;
using SukiUI.Toasts;
using System.Linq;
using System.Threading.Tasks;
using System;
using UseCases.Users;

namespace Desktop.ViewModels;

public partial class MainWindowViewModel(
    UserUseCases userUseCases
) : ViewModelBase
{
    public ISukiToastManager ToastManager { get; } = new SukiToastManager();
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }

    [RelayCommand]
    private async Task AddUserAsync()
    {
        var result = await userUseCases.AddUser.ExecuteAsync(new AddUserRequest(
            Email,
            Name,
            Age
        ));

        if (result.Success)
        {
            this.ShowToast("User added successfully.", NotificationType.Success);
        }
        else
        {
            var errorMessage = string.Join("\n", result.Errors.Select(e => e.Message));
            this.ShowToast(errorMessage, NotificationType.Error);
        }
    }

    private void ShowToast(
        string message,
        NotificationType toastType,
        bool autoDismiss = true)
    {
        var toastBuilder = ToastManager.CreateToast()
            .WithTitle($"{toastType}")
            .WithContent(message)
            .OfType(toastType)
            .Dismiss().ByClicking();

        if (toastType != NotificationType.Error || autoDismiss)
        {
            toastBuilder.Dismiss().After(TimeSpan.FromSeconds(5));
        }

        toastBuilder.Queue();
    }
}
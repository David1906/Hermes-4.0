using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.Input;
using Domain.Users;
using SukiUI.Toasts;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Threading;
using Common;
using Domain.Operations;
using ROP;
using UseCases.Operations;
using UseCases.Users;

namespace Desktop.ViewModels;

public partial class MainWindowViewModel(
    UserUseCases userUseCases,
    OperationsUseCases operationsUseCases,
    IResilientFileSystem resilientFileSystem // TODO: Remove this dependency if not needed
) : ViewModelBase
{
    public ISukiToastManager ToastManager { get; } = new SukiToastManager();
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }

    [RelayCommand]
    private async Task SendOperationAsync(CancellationToken ct = default)
    {
        Console.WriteLine($@"Start: {DateTime.Now:HH:mm:ss.fff}");
        var newFile = await resilientFileSystem.CopyFileAsync(
            @"C:\Users\david_ascencio\Documents\dev\Hermes\Input\PASS_4EA36.3dx",
            $@"C:\Users\david_ascencio\Documents\dev\Hermes\Input\PASS_4EA36_{Guid.NewGuid()}.3dx",
            ct
        );
        var a = await operationsUseCases.AddOperationToSfc.ExecuteAsync(
            new AddOperationToSfcRequest(
                FileToUpload: new FileInfo(newFile),
                OkResponses: "OK",
                Timeout: TimeSpan.FromSeconds(5),
                MaxRetries: 0),
            ct);
        await resilientFileSystem.DeleteFileIfExistsAsync(newFile, ct);
        await resilientFileSystem.DeleteFileIfExistsAsync(newFile.Replace(".3dx", ".log"), ct);
        a.Map(x =>
        {
            Console.WriteLine($"File: {x.UploadResponse} Result: {x.UploadResult}");
            return x;
        });
        Console.WriteLine($@"End: {DateTime.Now:HH:mm:ss.fff}");
        this.ShowToast("Operation sent successfully.", NotificationType.Success);
    }

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
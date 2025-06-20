using Avalonia.Controls.Notifications;
using Common;
using CommunityToolkit.Mvvm.Input;
using Domain.Core.Errors;
using Domain.Core.Types;
using Domain.Logfiles;
using Domain.Machines;
using Domain.Users;
using OneOf;
using R3;
using SukiUI.Toasts;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using UseCases.Logfiles;
using UseCases.Users;

namespace Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ISukiToastManager ToastManager { get; } = new SukiToastManager();
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public BindableReactiveProperty<StateType> MachineState { get; } = new(StateType.Stopped);
    public BindableReactiveProperty<bool> IsConnected { get; } = new(false);

    private readonly UserUseCases _userUseCases;
    private readonly LogfilesUseCases _logfilesUseCases;
    private readonly IResilientFileSystem _resilientFileSystem;
    private readonly IMachine _machine;

    public MainWindowViewModel(
        UserUseCases userUseCases,
        LogfilesUseCases logfilesUseCases,
        IResilientFileSystem resilientFileSystem, // TODO: Remove this dependency if not needed,
        IMachine machine)
    {
        this._userUseCases = userUseCases;
        this._logfilesUseCases = logfilesUseCases;
        this._resilientFileSystem = resilientFileSystem;
        this._machine = machine;
        this.SetupRx();
    }

    private void SetupRx()
    {
        this._machine.LogfileCreated
            .SubscribeAwait(async (result, ct) =>
            {
                await result.Match<Task>(
                    logfile => this.ProcessLogfile(logfile, ct),
                    this.ProcessError);
            })
            .AddTo(ref Disposables);

        this._machine.State
            .Do(x => this.MachineState.Value = x)
            .Subscribe(x => this.IsConnected.Value = x != StateType.Stopped)
            .AddTo(ref Disposables);
    }

    private async Task ProcessLogfile(Logfile logfile, CancellationToken ct)
    {
        await this.AddLogfileToSfc(logfile, ct);
    }

    private async Task<OneOf<Logfile, Error>> AddLogfileToSfc(Logfile logfile, CancellationToken ct)
    {
        Console.WriteLine($@"Start: {DateTime.Now:HH:mm:ss.fff}");
        var response = await _logfilesUseCases.AddLogfileToSfc.ExecuteAsync(
            new AddLogfileToSfcCommand(
                LogfileToUpload: logfile,
                OkResponses: "OK",
                Timeout: TimeSpan.FromSeconds(5),
                BackupDirectory: new DirectoryInfo(@"C:\Users\david_ascencio\Documents\dev\Hermes\Backup\"),
                MaxRetries: 0),
            ct);

        response.Switch(
            success =>
            {
                this.ShowToast($"File: {logfile.FileInfo.Name} Result: {success}",
                    NotificationType.Success);
            },
            error => { this.ShowToast($"Error: {error.Message}", NotificationType.Error); });
        Console.WriteLine($@"End: {DateTime.Now:HH:mm:ss.fff}");
        return response;
    }

    private Task ProcessError(Error error)
    {
        this.ShowToast($"Error : {error.Message}", NotificationType.Error);
        return Task.FromResult<OneOf<Logfile, Error>>(error);
    }

    [RelayCommand]
    private void Start()
    {
        _machine.Start();
        this.ShowToast("Started successfully.", NotificationType.Information);
    }

    [RelayCommand]
    private void Stop()
    {
        _machine.Stop();
        this.ShowToast("Stopped successfully.", NotificationType.Information);
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
            error => this.ShowToast(error.Message, NotificationType.Error)
        );
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
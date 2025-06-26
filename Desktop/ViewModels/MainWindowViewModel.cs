using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.Input;
using Domain.Core.Types;
using Domain.Logfiles;
using Domain.Machines;
using Domain.Users;
using R3;
using SukiUI.Toasts;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.ResultOf;
using Domain.Operations;
using Infrastructure.Data;
using UseCases.Operations;
using UseCases.Users;

namespace Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private const string BackupPath = @"C:\Users\david_ascencio\Documents\dev\Hermes\Backup";
    public ISukiToastManager ToastManager { get; } = new SukiToastManager();
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public BindableReactiveProperty<StateType> MachineState { get; } = new(StateType.Stopped);
    public BindableReactiveProperty<bool> IsConnected { get; } = new(false);

    private readonly UserUseCases _userUseCases;
    private readonly OperationsUseCases _operationsUseCases;
    private readonly IMachine _machine;

    public MainWindowViewModel(
        UserUseCases userUseCases,
        OperationsUseCases operationsUseCases,
        SqliteContext sqliteContext,
        IMachine machine)
    {
        this._userUseCases = userUseCases;
        this._operationsUseCases = operationsUseCases;
        this._machine = machine;
        sqliteContext.Migrate();
        this.SetupRx();
    }

    private void SetupRx()
    {
        this._machine.LogfileCreated
            .SubscribeAwait(async (inputLogfile, ct) =>
            {
                var res = await inputLogfile
                    .Bind(this.ProcessOperation, ct)
                    .OnSuccess(x =>
                        this.ShowToast($"Operation Id: {x.Id}, Sn:{x.MainSerialNumber}", NotificationType.Success))
                    .OnFailure(errors =>
                        this.ShowToast($"Errors : {ConcatErrorMessages(errors)}", NotificationType.Error));
            })
            .AddTo(ref Disposables);

        this._machine.State
            .Do(x => this.MachineState.Value = x)
            .Subscribe(x => this.IsConnected.Value = x != StateType.Stopped)
            .AddTo(ref Disposables);
    }

    private string ConcatErrorMessages(IEnumerable<Error> errors)
    {
        return string.Join("\n", errors.Select(x => x.Message));
    }

    private async Task<ResultOf<Operation>> ProcessOperation(Logfile logfile, CancellationToken ct)
    {
        Console.WriteLine($@"Start: {DateTime.Now:HH:mm:ss.fff}");
        var response = await _operationsUseCases.ProcessOperation.ExecuteAsync(
            new ProcessOperationCommand(
                InputLogfile: logfile,
                BackupDirectory: new DirectoryInfo(BackupPath),
                OkResponses: "OK",
                Timeout: TimeSpan.FromSeconds(5),
                MaxRetries: 0),
            ct);
        Console.WriteLine($@"End: {DateTime.Now:HH:mm:ss.fff}");
        return response;
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
            errors => this.ShowToast(ConcatErrorMessages(errors), NotificationType.Error)
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
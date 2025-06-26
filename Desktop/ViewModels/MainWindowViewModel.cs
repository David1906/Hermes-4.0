using Avalonia.Controls.Notifications;
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
using Common.Extensions;
using Domain.Operations;
using Infrastructure.Data;
using UseCases.Logfiles;
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
    private readonly LogfilesUseCases _logfilesUseCases;
    private readonly OperationsUseCases _operationsUseCases;
    private readonly IMachine _machine;
    private readonly TriOperationParser _triOperationParser;

    public MainWindowViewModel(
        UserUseCases userUseCases,
        LogfilesUseCases logfilesUseCases,
        OperationsUseCases operationsUseCases,
        TriOperationParser triOperationParser,
        SqliteContext sqliteContext,
        IMachine machine)
    {
        this._userUseCases = userUseCases;
        this._logfilesUseCases = logfilesUseCases;
        this._operationsUseCases = operationsUseCases;
        this._triOperationParser = triOperationParser;
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
                    .Bind(this.ProcessOperation, ct);

                res.Switch(
                    operation => this.ShowToast(
                        $"Operation Id: {operation.Id}, Sn:{operation.MainSerialNumber}",
                        NotificationType.Success),
                    error => this.ShowToast($"Error : {error.Message}", NotificationType.Error));
            })
            .AddTo(ref Disposables);

        this._machine.State
            .Do(x => this.MachineState.Value = x)
            .Subscribe(x => this.IsConnected.Value = x != StateType.Stopped)
            .AddTo(ref Disposables);
    }

    private async Task<OneOf<Operation, Error>> ProcessOperation(Logfile logfile, CancellationToken ct)
    {
        return await this.MoveLogfileToBackup(logfile, ct)
            .Combine(inputLogfile => this.AddLogfileToSfc(inputLogfile, ct))
            .Bind(this.AddOperation, ct);
    }

    private async Task<OneOf<Logfile, Error>> MoveLogfileToBackup(Logfile logfile, CancellationToken ct)
    {
        return await this._logfilesUseCases.MoveLogfileToBackup.ExecuteAsync(new MoveLogfileToBackupCommand(
            Logfile: logfile,
            DestinationDirectory: new DirectoryInfo(BackupPath)
        ), ct);
    }

    private async Task<OneOf<Logfile, Error>> AddLogfileToSfc(Logfile logfile, CancellationToken ct)
    {
        Console.WriteLine($@"Start: {DateTime.Now:HH:mm:ss.fff}");
        var response = await _logfilesUseCases.AddLogfileToSfc.ExecuteAsync(
            new AddLogfileToSfcCommand(
                LogfileToUpload: logfile,
                OkResponses: "OK",
                Timeout: TimeSpan.FromSeconds(5),
                BackupDirectory: new DirectoryInfo(BackupPath),
                MaxRetries: 0),
            ct);
        Console.WriteLine($@"End: {DateTime.Now:HH:mm:ss.fff}");
        return response;
    }

    private async Task<OneOf<Operation, Error>> AddOperation(
        (Logfile input, Logfile response) logfiles,
        CancellationToken ct)
    {
        var operation = this._triOperationParser.Parse(logfiles.input);
        operation.UploadLogfile = logfiles.response;
        return await this._operationsUseCases.AddOperation.ExecuteAsync(new AddOperationCommand(
            operation
        ), ct);
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
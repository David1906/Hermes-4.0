using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Common.ResultOf;
using CommunityToolkit.Mvvm.Input;
using Core.Application.Features.Panels.Commands;
using Core.Domain;
using Desktop.Common;
using Desktop.Core;
using Desktop.Core.Types;
using Desktop.Extensions;
using Desktop.Features.Machines.Domain;
using Desktop.Features.Operations.Domain;
using Desktop.Features.Operations.UseCases;
using Desktop.Features.OperationTasks.UseCases;
using Desktop.Handlers;
using Paramore.Brighter;
using R3;
using Logfile = Desktop.Features.Logfiles.Domain.Logfile;
using LogfileType = Core.Domain.LogfileType;

namespace Desktop.Features.Operations.Delivery;

public partial class OperationProcessorViewModel : ViewModelBase
{
    private const string BackupPath = @"C:\Users\david_ascencio\Documents\dev\Hermes\Backup";

    public BindableReactiveProperty<StateType> MachineState { get; } = new(StateType.Stopped);
    public BindableReactiveProperty<bool> IsConnected { get; } = new(false);

    private readonly OperationsUseCases _operationsUseCases;
    private readonly EventsHandler _eventsHandler;
    private readonly ViewModelFactory _viewModelFactory;
    private readonly IAmACommandProcessor _commandProcessor;
    private readonly IMachine _machine;

    public OperationProcessorViewModel(
        OperationsUseCases operationsUseCases,
        EventsHandler eventsHandler,
        ViewModelFactory viewModelFactory,
        IAmACommandProcessor commandProcessor,
        IMachine machine)
    {
        this._operationsUseCases = operationsUseCases;
        this._eventsHandler = eventsHandler;
        this._viewModelFactory = viewModelFactory;
        this._commandProcessor = commandProcessor;
        this._machine = machine;
        this.SetupRx();
    }

    private void SetupRx()
    {
        this._machine.LogfileCreated
            .SubscribeAwait(async (inputLogfile, ct) =>
            {
                await inputLogfile
                    .Bind(this.ProcessOperation, ct)
                    .OnFailure(x =>
                    {
                        this._commandProcessor.ShowErrorToast(x.Message);
                        return Task.CompletedTask;
                    });
            })
            .AddTo(ref Disposables);

        this._machine.State
            .Do(x => this.MachineState.Value = x)
            .Subscribe(x => this.IsConnected.Value = x != StateType.Stopped)
            .AddTo(ref Disposables);

        this._eventsHandler.OperationTaskCreated
            .SubscribeAwait(async (x, ct) => await this.OnOperationTaskCreated(x, ct))
            .AddTo(ref Disposables);

        this._eventsHandler.OperationCreated
            .Subscribe(x => this.OnOperationCreated(x))
            .AddTo(ref Disposables);
    }

    private async Task<ResultOf<Panel>> ProcessOperation(Logfile logfile, CancellationToken ct)
    {
        Console.WriteLine($@"Start: {DateTime.Now:HH:mm:ss.fff}");
        var cmd = new ProcessPanelFromLogfileCommand()
        {
            InputLogfile = new global::Core.Domain.Logfile()
            {
                FileInfo = logfile.FileInfo,
                Type = LogfileType.TriDefault
            },
            BackupDirectory = new DirectoryInfo(BackupPath),
            OkResponses = "OK",
            Timeout = TimeSpan.FromSeconds(5),
            MaxRetries = 0
        };
        await _commandProcessor.SendAsync(cmd, cancellationToken: ct);
        Console.WriteLine($@"End: {DateTime.Now:HH:mm:ss.fff}");
        return cmd.Result;
    }

    private async Task OnOperationTaskCreated(OperationTaskCreatedEvent @event, CancellationToken ct)
    {
        if (@event.OperationTask is { Type: OperationTaskType.AddLogfileToSfc, Result: OperationTaskResultType.Pass })
        {
            await _machine.SendAcknowledgmentAsync(@event.MainSerialNumber);
            this._commandProcessor.ShowSuccessToast($"AddLogfileToSfc success {@event.MainSerialNumber}.");
            await this._viewModelFactory.CreateSuccess(@event.MainSerialNumber)
                .OnSuccess(viewModel => this._commandProcessor.OpenWindow(viewModel));
        }
        else if (@event.OperationTask.Type == OperationTaskType.AddLogfileToSfc)
        {
            this._commandProcessor.ShowErrorToast(
                $"AddLogfileToSfc error {@event.MainSerialNumber}\n{@event.OperationTask.Message}.");

            await this._viewModelFactory.CreateStopAsync(@event.OperationTask)
                .OnSuccess(viewModel => this._commandProcessor.OpenWindow(viewModel));
        }
    }

    private void OnOperationCreated(OperationCreatedEvent @event)
    {
        this._commandProcessor.ShowInformationToast(
            $"Operation added {@event.MainSerialNumber}.");
    }

    [RelayCommand]
    private void Start()
    {
        _machine.Start();
        this._commandProcessor.ShowInformationToast("Started successfully.");
    }

    [RelayCommand]
    private void Stop()
    {
        _machine.Stop();
        this._commandProcessor.ShowInformationToast("Stopped successfully.");
    }
}
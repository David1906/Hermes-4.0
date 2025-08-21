using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common.ResultOf;
using CommunityToolkit.Mvvm.Input;
using Core.Application.Common.Events;
using Core.Application.Common.Extensions;
using Core.Application.Features.Panels.Commands;
using Core.Domain;
using Core.Domain.Common.Types;
using Paramore.Brighter;
using R3;
using UI.Common;
using UI.Handlers;
using UI.Machines;

namespace UI.Features.Panels;

public partial class PanelProcessorViewModel : ViewModelBase
{
    private const string BackupPath = @"C:\Users\david_ascencio\Documents\dev\Hermes\Backup";

    public BindableReactiveProperty<StateType> MachineState { get; } = new(StateType.Stopped);
    public BindableReactiveProperty<bool> IsConnected { get; } = new(false);

    private readonly EventsHandler _eventsHandler;
    private readonly IAmACommandProcessor _commandProcessor;
    private readonly IMachine _machine;

    public PanelProcessorViewModel(
        EventsHandler eventsHandler,
        IAmACommandProcessor commandProcessor,
        IMachine machine)
    {
        this._eventsHandler = eventsHandler;
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

        this._eventsHandler.OperationCreated
            .SubscribeAwait(async (x, ct) => await this.OnOperationCreated(x, ct))
            .AddTo(ref Disposables);
    }

    private async Task<ResultOf<Panel>> ProcessOperation(Logfile logfile, CancellationToken ct)
    {
        Console.WriteLine($@"Start: {DateTime.Now:HH:mm:ss.fff}");
        var cmd = new ProcessPanelFromLogfileCommand()
        {
            InputLogfile = new Logfile()
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

    private async Task OnOperationCreated(OperationCreatedEvent @event, CancellationToken _)
    {
        if (@event.Operation is { Type: OperationType.SendPanelToNextStation, IsPass: true })
        {
            await _machine.SendAcknowledgmentAsync(@event.MainSerialNumber);
        }
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
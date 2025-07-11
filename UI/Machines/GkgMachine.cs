using Common.Extensions;
using Common.ResultOf;
using Common.Serial;
using Common;
using Core.Application.Common.Errors;
using Core.Application.Common.FileBuilders;
using Core.Domain;
using Infrastructure.ExternalDevices;
using R3;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using Core.Domain.Common.Types;

namespace UI.Machines;

public class GkgMachine : IMachine, IDisposable
{
    public Subject<ResultOf<Logfile>> LogfileCreated { get; } = new();
    public BehaviorSubject<StateType> State { get; } = new(StateType.Stopped);

    private readonly SerialPortRx _machineSerialPortRx;
    private readonly SerialScannerRx _serialScanner;
    private readonly IResilientFileSystem _fileSystem;
    private readonly ITriLogFileBuilder _triLogFileBuilder;
    private readonly GkgMachineOptions _gkgMachineOptions;
    private readonly MachineOptions _machineOptions;
    private DisposableBag _disposables;

    public GkgMachine(
        SerialPortRx machineSerialPortRx,
        SerialScannerRx serialScanner,
        IResilientFileSystem fileSystem,
        ITriLogFileBuilder triLogFileBuilder,
        GkgMachineOptions gkgMachineGkgMachineOptions,
        MachineOptions machineOptions)
    {
        this._machineSerialPortRx = machineSerialPortRx;
        this._serialScanner = serialScanner;
        this._fileSystem = fileSystem;
        this._triLogFileBuilder = triLogFileBuilder;
        this._gkgMachineOptions = gkgMachineGkgMachineOptions;
        this._machineOptions = machineOptions;
        this.SetupSerialPorts();
        this.SetupRx();
    }

    private void SetupSerialPorts()
    {
        this._machineSerialPortRx.Options = _gkgMachineOptions.SerialPortOptions;

        this._serialScanner.Options = _gkgMachineOptions.ScannerOptions.SerialPortOptions;
        this._serialScanner.TriggerOnCommand = _gkgMachineOptions.ScannerOptions.TriggerOnCommand;
        this._serialScanner.TriggerOffCommand = _gkgMachineOptions.ScannerOptions.TriggerOffCommand;
        this._serialScanner.LineTerminator = _gkgMachineOptions.ScannerOptions.LineTerminator;
        this._serialScanner.ErrorScanText = _gkgMachineOptions.ScannerOptions.ErrorScanText;
    }

    private void SetupRx()
    {
        this._machineSerialPortRx.DataReceived
            .Where(x => x.Contains(_gkgMachineOptions.TriggerOnCommand))
            .ThrottleFirst(TimeSpan.FromSeconds(10))
            .SubscribeAwait(async (_, ct) => await this.OnTriggerReceivedAsync(ct))
            .AddTo(ref _disposables);

        this._serialScanner
            .State
            .Where(x => x == StateType.Stopped)
            .Subscribe(_ => this.Stop())
            .AddTo(ref _disposables);
    }

    private async Task OnTriggerReceivedAsync(CancellationToken ct)
    {
        this.State.OnNext(StateType.Scanning);
        var serialNumber = await this._serialScanner.ScanAsync();
        if (this._serialScanner.IsReadError(serialNumber))
        {
            // TODO: Open stop window
            this.LogfileCreated.OnNext(ResultOf<Logfile>.Failure(new ScanningError()));
        }
        else
        {
            await _triLogFileBuilder
                .Clone()
                .SerialNumber(serialNumber)
                .StationId(this._machineOptions.StationId)
                .WriteAsync(GetInputLogfileFullPath(serialNumber), ct)
                .OnSuccess(inputLogfilePath => this.LogfileCreated.OnNext(
                    new Logfile
                    {
                        FileInfo = new FileInfo(inputLogfilePath)
                    }));
        }

        this.State.OnNext(StateType.Idle);
    }

    private string GetInputLogfileFullPath(string serialNumber)
    {
        return Path.Combine(
            this._machineOptions.LogfileDirectory.FullName,
            $"{serialNumber.Trim()}_{DateTime.Now:hh_mm_ss}{this._machineOptions.LogFileExtension.GetDescription()}");
    }

    public void Start()
    {
        this._machineSerialPortRx.Open();
        this._serialScanner.Open();
        this.State.OnNext(StateType.Idle);
    }

    public void Stop()
    {
        this._machineSerialPortRx.Close();
        this._serialScanner.Close();
        this.State.OnNext(StateType.Stopped);
    }

    public async Task SendAcknowledgmentAsync(string serialNumber)
    {
        Debug.Assert(!string.IsNullOrEmpty(serialNumber));

        await _machineSerialPortRx.WriteAsync(serialNumber);
    }

    public void Dispose()
    {
        this._disposables.Dispose();
        this._machineSerialPortRx.Dispose();
    }
}
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Extensions;
using Common.ResultOf;
using Common.Serial;
using Core.Application.Common.Errors;
using Core.Application.Common.ExternalDevices;
using Core.Application.Common.FileBuilders;
using Core.Domain;
using Core.Domain.Common.Types;
using R3;

namespace UI.Machines;

public class GkgMachine : IMachine, IDisposable
{
    private readonly IResilientFileSystem _fileSystem;
    private readonly GkgMachineOptions _gkgMachineOptions;
    private readonly MachineOptions _machineOptions;

    private readonly SerialPortRx _machineSerialPortRx;
    private readonly ISerialScannerRx _serialScanner;
    private readonly ITriLogFileBuilder _triLogFileBuilder;
    private DisposableBag _disposables;

    public GkgMachine(
        SerialPortRx machineSerialPortRx,
        ISerialScannerRx serialScanner,
        IResilientFileSystem fileSystem,
        ITriLogFileBuilder triLogFileBuilder,
        GkgMachineOptions gkgMachineGkgMachineOptions,
        MachineOptions machineOptions)
    {
        _machineSerialPortRx = machineSerialPortRx;
        _serialScanner = serialScanner;
        _fileSystem = fileSystem;
        _triLogFileBuilder = triLogFileBuilder;
        _gkgMachineOptions = gkgMachineGkgMachineOptions;
        _machineOptions = machineOptions;
        SetupSerialPorts();
        SetupRx();
    }

    public void Dispose()
    {
        _disposables.Dispose();
        _machineSerialPortRx.Dispose();
    }

    public Subject<ResultOf<Logfile>> LogfileCreated { get; } = new();
    public BehaviorSubject<StateType> State { get; } = new(StateType.Stopped);

    public void Start()
    {
        _machineSerialPortRx.Open();
        _serialScanner.Open();
        State.OnNext(StateType.Idle);
    }

    public void Stop()
    {
        _machineSerialPortRx.Close();
        _serialScanner.Close();
        State.OnNext(StateType.Stopped);
    }

    public async Task SendAcknowledgmentAsync(string serialNumber)
    {
        Debug.Assert(!string.IsNullOrEmpty(serialNumber));

        await _machineSerialPortRx.WriteAsync(serialNumber);
    }

    private void SetupSerialPorts()
    {
        _machineSerialPortRx.Options = _gkgMachineOptions.SerialPortOptions;

        _serialScanner.Options = _gkgMachineOptions.ScannerOptions.SerialPortOptions;
        _serialScanner.TriggerOnCommand = _gkgMachineOptions.ScannerOptions.TriggerOnCommand;
        _serialScanner.TriggerOffCommand = _gkgMachineOptions.ScannerOptions.TriggerOffCommand;
        _serialScanner.LineTerminator = _gkgMachineOptions.ScannerOptions.LineTerminator;
        _serialScanner.ErrorScanText = _gkgMachineOptions.ScannerOptions.ErrorScanText;
    }

    private void SetupRx()
    {
        _machineSerialPortRx.DataReceived
            .Where(x => x.Contains(_gkgMachineOptions.TriggerOnCommand))
            .ThrottleFirst(TimeSpan.FromSeconds(10))
            .SubscribeAwait(async (_, ct) => await OnTriggerReceivedAsync(ct))
            .AddTo(ref _disposables);

        _serialScanner
            .State
            .Where(x => x == StateType.Stopped)
            .Subscribe(_ => Stop())
            .AddTo(ref _disposables);
    }

    private async Task OnTriggerReceivedAsync(CancellationToken ct)
    {
        State.OnNext(StateType.Scanning);
        var serialNumber = await _serialScanner.ScanAsync();
        if (_serialScanner.IsReadError(serialNumber))
            // TODO: Open stop window
            LogfileCreated.OnNext(ResultOf<Logfile>.Failure(new ScanningError()));
        else
            await _triLogFileBuilder
                .Clone()
                .SerialNumber(serialNumber)
                .StationId(_machineOptions.StationId)
                .WriteAsync(GetInputLogfileFullPath(serialNumber), ct)
                .OnSuccess(inputLogfilePath => LogfileCreated.OnNext(
                    new Logfile
                    {
                        FileInfo = new FileInfo(inputLogfilePath)
                    }));

        State.OnNext(StateType.Idle);
    }

    private string GetInputLogfileFullPath(string serialNumber)
    {
        return Path.Combine(
            _machineOptions.LogfileDirectory.FullName,
            $"{serialNumber.Trim()}_{DateTime.Now:hh_mm_ss}{_machineOptions.LogFileExtension.GetDescription()}");
    }
}
using Common;
using Common.Extensions;
using Common.Serial;
using Domain.Builders;
using Domain.Core.Errors;
using Domain.Core.Types;
using Domain.Logfiles;
using Domain.Machines;
using Infrastructure.Scanners;
using OneOf;
using R3;

namespace Infrastructure.Machines;

public class GkgMachine : IMachine, IDisposable
{
    public Subject<OneOf<Logfile, Error>> LogfileCreated { get; } = new();
    public BehaviorSubject<StateType> State { get; } = new(StateType.Stopped);

    private readonly SerialPortRx _serialPortRx;
    private readonly SerialScannerRx _serialScanner;
    private readonly IResilientFileSystem _fileSystem;
    private readonly GkgMachineOptions _gkgMachineOptions;
    private readonly MachineOptions _machineOptions;
    private DisposableBag _disposables;

    public GkgMachine(
        SerialPortRx serialPortRx,
        SerialScannerRx serialScanner,
        IResilientFileSystem fileSystem,
        GkgMachineOptions gkgMachineGkgMachineOptions,
        MachineOptions machineOptions)
    {
        this._serialPortRx = serialPortRx;
        this._serialScanner = serialScanner;
        this._fileSystem = fileSystem;
        this._gkgMachineOptions = gkgMachineGkgMachineOptions;
        this._machineOptions = machineOptions;
        this.SetupSerialPorts();
        this.SetupRx();
    }

    private void SetupSerialPorts()
    {
        this._serialPortRx.Options = _gkgMachineOptions.SerialPortOptions;

        this._serialScanner.Options = _gkgMachineOptions.ScannerOptions.SerialPortOptions;
        this._serialScanner.TriggerOnCommand = _gkgMachineOptions.ScannerOptions.TriggerOnCommand;
        this._serialScanner.TriggerOffCommand = _gkgMachineOptions.ScannerOptions.TriggerOffCommand;
        this._serialScanner.LineTerminator = _gkgMachineOptions.ScannerOptions.LineTerminator;
        this._serialScanner.ErrorScanText = _gkgMachineOptions.ScannerOptions.ErrorScanText;
    }

    private void SetupRx()
    {
        this._serialPortRx.DataReceived
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
            this.LogfileCreated.OnNext(Error.ScanningError);
        }
        else
        {
            var logfileText = new TriLogfileBuilder()
                .SerialNumber(serialNumber)
                .StationId(this._machineOptions.StationId)
                .Build();
            var logfile = await this.CreateAndWriteLogfileAsync(serialNumber, logfileText, ct);
            this.LogfileCreated.OnNext(logfile);
        }

        this.State.OnNext(StateType.Idle);
    }

    private async Task<Logfile> CreateAndWriteLogfileAsync(
        string serialNumber,
        string logfileText,
        CancellationToken ct)
    {
        var logfileFullpath = Path.Combine(
            this._machineOptions.LogfileDirectory.FullName,
            $"{serialNumber.Trim()}_{DateTime.Now:hh_mm_ss}{this._machineOptions.LogFileExtension.GetDescription()}");
        await this._fileSystem.WriteAllTextAsync(logfileFullpath, logfileText, ct);
        return new Logfile
        {
            Content = logfileText,
            FileInfo = new FileInfo(logfileFullpath)
        };
    }

    public void Start()
    {
        this._serialPortRx.Open();
        this._serialScanner.Open();
        this.State.OnNext(StateType.Idle);
    }

    public void Stop()
    {
        this._serialPortRx.Close();
        this._serialScanner.Close();
        this.State.OnNext(StateType.Stopped);
    }

    public void Dispose()
    {
        this._disposables.Dispose();
        this._serialPortRx.Dispose();
    }
}
using Common.Serial;
using Domain.Core.Types;
using R3;

namespace Data.Machines;

public class GkgMachineService : MachineService, IDisposable
{
    private readonly SerialPortRx _serialPortRx;
    private readonly SerialScannerRx _serialScanner;
    private readonly GkgMachineOptions _options;
    private DisposableBag _disposables;

    public GkgMachineService(
        SerialPortRx serialPortRx,
        SerialScannerRx serialScanner,
        GkgMachineOptions gkgMachineOptions,
        StationOptions stationOptions)
    {
        this._serialPortRx = serialPortRx;
        this._serialScanner = serialScanner;
        this._options = gkgMachineOptions;
        this.SetupSerialPorts();
        this.SetupRx();
    }

    private void SetupSerialPorts()
    {
        this._serialPortRx.Options = _options.SerialPortOptions;

        this._serialScanner.Options = _options.ScannerOptions.SerialPortOptions;
        this._serialScanner.TriggerOnCommand = _options.ScannerOptions.TriggerOnCommand;
        this._serialScanner.TriggerOffCommand = _options.ScannerOptions.TriggerOffCommand;
        this._serialScanner.LineTerminator = _options.ScannerOptions.LineTerminator;
        this._serialScanner.ErrorScanText = _options.ScannerOptions.ErrorScanText;
    }

    private void SetupRx()
    {
        this._serialPortRx.DataReceived
            .Where(x => x.Contains(_options.TriggerOnCommand))
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
        this.State.Value = StateType.Scanning;
        var serialNumber = await this._serialScanner.ScanAsync();
        this.State.Value = StateType.Idle;
    }

    public override void Start()
    {
        this._serialPortRx.Open();
        this._serialScanner.Open();
        this.State.Value = StateType.Idle;
    }

    public override void Stop()
    {
        this._serialPortRx.Close();
        this._serialScanner.Close();
        this.State.Value = StateType.Stopped;
    }

    public void Dispose()
    {
        this._disposables.Dispose();
        this._serialPortRx.Dispose();
    }
}
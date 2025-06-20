using System.IO.Ports;
using R3;

namespace Common.Serial;

public class SerialPortRx : SerialPortAsync
{
    public Observable<string> DataReceived { get; private set; }
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public SerialPortRx() : this(null, null)
    {
    }

    public SerialPortRx(SerialPort? serialPort, SerialPortOptions? serialPortOptions)
        : base(serialPort, serialPortOptions)
    {
        this.DataReceived = Observable.FromEvent<SerialDataReceivedEventHandler, SerialDataReceivedEventArgs>(
                h => (sender, e) => h(e),
                x => this.SerialPort.DataReceived += x,
                x => this.SerialPort.DataReceived -= x)
            .TakeUntil(this._cancellationTokenSource.Token)
            .Delay(TimeSpan.FromMilliseconds(20))
            .Select(x => this.ReadExisting());
    }

    public new void Dispose()
    {
        base.Dispose();
        this._cancellationTokenSource.Cancel();
    }
}
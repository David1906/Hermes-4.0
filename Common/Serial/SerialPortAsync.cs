using System.IO.Ports;
using System.Text;

namespace Common.Serial;

public class SerialPortAsync : IDisposable
{
    private SerialPortOptions _serialPortOptions = null!;

    public SerialPortOptions Options
    {
        get => _serialPortOptions;
        set
        {
            this._serialPortOptions = value;
            this.SerialPort.PortName = value.PortName;
            this.SerialPort.BaudRate = (int)value.BaudRate;
            this.SerialPort.DataBits = (int)value.DataBits;
            this.SerialPort.Parity = value.Parity;
            this.SerialPort.StopBits = value.StopBits;
            this.SerialPort.Handshake = value.Handshake;
            this.SerialPort.ReadTimeout = value.ReadTimeout;
            this.SerialPort.WriteTimeout = value.WriteTimeout;
        }
    }

    protected readonly SerialPort SerialPort;
    private bool _isWaitingForData;


    public SerialPortAsync() : this(null, null)
    {
    }

    public SerialPortAsync(SerialPort? serialPort, SerialPortOptions? serialPortOptions)
    {
        this.SerialPort = serialPort ?? new SerialPort();
        this.SerialPort.DataReceived += OnDataReceived;
        this.Options = serialPortOptions ?? new SerialPortOptions();
    }

    private void OnDataReceived(object _, SerialDataReceivedEventArgs __)
    {
        _isWaitingForData = false;
    }

    public bool IsOpen => this.SerialPort.IsOpen;

    public void Open()
    {
        if (this.SerialPort.IsOpen) return;
        this.SerialPort.Open();
        this.SerialPort.DiscardInBuffer();
        this.SerialPort.DiscardOutBuffer();
    }

    public void Close()
    {
        if (!this.SerialPort.IsOpen) return;
        SerialPort.Close();
    }

    public void Write(string text)
    {
        SerialPort.Write(text);
    }

    public async Task WriteAsync(string command)
    {
        if (SerialPort is not { IsOpen: true }) return;
        using var cts = new CancellationTokenSource(this.Options.WriteTimeout);
        await SerialPort.BaseStream.WriteAsync(Encoding.ASCII.GetBytes(command), cts.Token);
    }

    public string ReadExisting()
    {
        if (SerialPort is not { IsOpen: true }) return string.Empty;

        try
        {
            return SerialPort.ReadExisting();
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public async Task<string> WaitForDataAsync()
    {
        if (SerialPort is not { IsOpen: true }) return string.Empty;

        try
        {
            _isWaitingForData = true;
            using var cts = new CancellationTokenSource(this.Options.ReadTimeout);
            while (_isWaitingForData && !cts.IsCancellationRequested)
            {
                await Task.Delay(50, cts.Token);
            }

            return await ReadExistingAsync();
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }


    public async Task<string> ReadExistingAsync()
    {
        if (SerialPort is not { IsOpen: true }) return string.Empty;

        try
        {
            var expectedBytes = SerialPort.BytesToRead;
            if (expectedBytes == 0)
            {
                return string.Empty;
            }

            var buffer = new byte[expectedBytes];
            var readBytes = 0;
            using var memoryStream = new MemoryStream();
            using var cts = new CancellationTokenSource(500);

            do
            {
                readBytes += await SerialPort.BaseStream.ReadAsync(
                    buffer.AsMemory(readBytes, expectedBytes), cts.Token);
                memoryStream.Write(buffer, 0, readBytes);
            } while (readBytes < expectedBytes);

            return Encoding.ASCII.GetString(memoryStream.ToArray());
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public void Dispose()
    {
        this.SerialPort.DataReceived -= OnDataReceived;
        this.SerialPort.Dispose();
    }

    public void DiscardInBuffer()
    {
        if (this.SerialPort.IsOpen)
        {
            this.SerialPort.DiscardInBuffer();
        }
    }
}
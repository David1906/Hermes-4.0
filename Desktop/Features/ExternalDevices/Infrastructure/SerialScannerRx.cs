using Common.Serial;
using Desktop.Core.Types;
using Desktop.Features.ExternalDevices.Domain;
using R3;
using System.Threading.Tasks;
using System;

namespace Desktop.Features.ExternalDevices.Infrastructure;

public class SerialScannerRx : ISerialScannerRx
{
    public string ErrorScanText { get; set; } = "ERROR";
    public string TriggerOnCommand { get; set; } = "LOF";
    public string TriggerOffCommand { get; set; } = "LON";
    public string LineTerminator { get; set; } = "\r";

    public ReactiveProperty<StateType> State { get; } = new(StateType.Stopped);
    public Subject<string> ScannedText { get; } = new();

    public SerialPortOptions Options
    {
        get => _serialPort.Options;
        set => _serialPort.Options = value;
    }

    private readonly SerialPortAsync _serialPort;

    public SerialScannerRx(SerialPortAsync? serialPortRx)
    {
        this._serialPort = serialPortRx ?? new SerialPortRx();
    }

    public void Open()
    {
        try
        {
            if (_serialPort is { IsOpen: true }) return;
            this._serialPort.Open();
            this.State.Value = StateType.Idle;
        }
        catch (Exception)
        {
            this.Close();
            throw;
        }
    }

    public void Close()
    {
        try
        {
            this._serialPort.Close();
        }
        finally
        {
            this.State.Value = StateType.Stopped;
        }
    }

    public async Task<string> ScanAsync()
    {
        if (_serialPort is not { IsOpen: true }) return "";

        this.State.Value = StateType.Processing;

        this._serialPort.DiscardInBuffer();
        await this._serialPort.WriteAsync(this.TriggerOnCommand + LineTerminator);
        var scannedText = await this._serialPort.WaitForDataAsync();
        scannedText = string.IsNullOrWhiteSpace(scannedText)
            ? ErrorScanText
            : scannedText;

        ScannedText.OnNext(scannedText);
        this.State.Value = StateType.Idle;
        return scannedText.Trim();
    }

    public bool IsReadError(string serialNumber)
    {
        return this.ErrorScanText.Equals(serialNumber);
    }
}
namespace Common.Serial;

public class ScannerOptions
{
    public SerialPortOptions SerialPortOptions { get; set; } = new();
    public string TriggerOnCommand { get; set; } = "LOF";
    public string TriggerOffCommand { get; set; } = "LON";
    public string LineTerminator { get; set; } = "\r";
    public string ErrorScanText { get; set; } = "ERROR";

    public ScannerOptions()
    {
        this.SerialPortOptions.PortName = "COM30";
    }
}
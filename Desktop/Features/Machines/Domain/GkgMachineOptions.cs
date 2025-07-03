using Common.Serial;

namespace Desktop.Features.Machines.Domain;

public class GkgMachineOptions
{
    public string TriggerOnCommand { get; set; } = "LOF";
    public SerialPortOptions SerialPortOptions { get; set; } = new();
    public ScannerOptions ScannerOptions { get; set; } = new();

    public GkgMachineOptions()
    {
        this.SerialPortOptions.PortName = "COM10";
        this.ScannerOptions.SerialPortOptions.PortName = "COM30";
    }
}
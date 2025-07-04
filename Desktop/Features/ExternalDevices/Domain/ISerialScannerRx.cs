using System.Threading.Tasks;
using Common.Serial;
using Desktop.Core.Types;
using R3;

namespace Desktop.Features.ExternalDevices.Domain;

public interface ISerialScannerRx
{
    string ErrorScanText { get; set; }
    string TriggerOnCommand { get; set; }
    string TriggerOffCommand { get; set; }
    string LineTerminator { get; set; }
    ReactiveProperty<StateType> State { get; }
    Subject<string> ScannedText { get; }
    SerialPortOptions Options { get; set; }
    void Open();
    void Close();
    Task<string> ScanAsync();
    bool IsReadError(string serialNumber);
}
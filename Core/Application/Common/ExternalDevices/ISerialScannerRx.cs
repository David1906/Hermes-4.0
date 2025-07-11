using Common.Serial;
using Core.Domain.Common.Types;
using R3;

namespace Core.Application.Common.ExternalDevices;

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
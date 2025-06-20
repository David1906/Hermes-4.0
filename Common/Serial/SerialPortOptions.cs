using System.IO.Ports;

namespace Common.Serial;

public class SerialPortOptions
{
    public string PortName { get; set; } = "COM1";
    public BaudRate BaudRate { get; set; } = BaudRate._9600;
    public DataBits DataBits { get; set; } = DataBits.Eight;
    public Parity Parity { get; set; } = Parity.Odd;
    public StopBits StopBits { get; set; } = StopBits.One;
    public Handshake Handshake { get; set; } = Handshake.None;
    public int ReadTimeout { get; set; } = 5000;
    public int WriteTimeout { get; set; } = 500;
}
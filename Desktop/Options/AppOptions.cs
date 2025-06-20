using System.IO;
using Domain.Core.Types;
using Domain.Logfiles;

namespace Desktop.Options;

public class AppOptions
{
    public LogfileGatewayOptions LogfileGatewayOptions { get; set; } = new()
    {
        BaseDirectory = new DirectoryInfo(@"C:\Users\david_ascencio\Documents\dev\Hermes\Sfc"),
        ResponseExtensionType = FileExtensionType.Log
    };
}
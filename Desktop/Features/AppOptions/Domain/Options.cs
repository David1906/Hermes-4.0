using System.IO;
using Desktop.Core.Types;
using Desktop.Features.Logfiles.Domain;

namespace Desktop.Features.AppOptions.Domain;

public class Options
{
    public LogfilesGatewayOptions LogfilesGatewayOptions { get; set; } = new()
    {
        BaseDirectory = new DirectoryInfo(@"C:\Users\david_ascencio\Documents\dev\Hermes\Sfc"),
        ResponseExtensionType = FileExtensionType.Log
    };
}
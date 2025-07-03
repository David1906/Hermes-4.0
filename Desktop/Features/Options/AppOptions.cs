using System.IO;
using Domain.Core.Types;
using Domain.Logfiles;

namespace Desktop.Features.Options;

public class AppOptions
{
    public LogfilesGatewayOptions LogfilesGatewayOptions { get; set; } = new()
    {
        BaseDirectory = new DirectoryInfo(@"C:\Users\david_ascencio\Documents\dev\Hermes\Sfc"),
        ResponseExtensionType = FileExtensionType.Log
    };
}
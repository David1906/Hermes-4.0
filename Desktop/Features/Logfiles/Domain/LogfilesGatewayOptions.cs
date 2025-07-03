using System.IO;
using Desktop.Core.Types;

namespace Desktop.Features.Logfiles.Domain;

public class LogfilesGatewayOptions
{
    public required DirectoryInfo BaseDirectory { get; set; } =
        new DirectoryInfo(@"C:\Users\david_ascencio\Documents\dev\Hermes\Sfc");

    public required FileExtensionType ResponseExtensionType { get; set; }
}
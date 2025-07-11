using Core.Domain.Common.Types;

namespace Core.Application.Common.Gateways;

public class SfcSharedFolderGatewayOptions
{
    public required DirectoryInfo BaseDirectory { get; set; } =
        new DirectoryInfo(@"C:\Users\david_ascencio\Documents\dev\Hermes\Sfc");

    public required FileExtensionType ResponseExtensionType { get; set; } = FileExtensionType.Log;
}
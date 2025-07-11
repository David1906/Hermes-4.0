using System.IO;
using Core.Application.Common.Gateways;
using Core.Domain.Common.Types;

namespace UI.Features.AppOptions;

public class Options
{
    public SfcSharedFolderGatewayOptions LogfilesGatewayOptions { get; set; } = new()
    {
        BaseDirectory = new DirectoryInfo(@"C:\Users\david_ascencio\Documents\dev\Hermes\Sfc"),
        ResponseExtensionType = FileExtensionType.Log
    };
}
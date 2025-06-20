using System.IO;
using Data.Sfc;
using Domain.Core.Types;

namespace Desktop.Options;

public class AppOptions
{
    public SfcApiOptions SfcApiOptions { get; set; } = new()
    {
        BaseDirectory = new DirectoryInfo(@"C:\Users\david_ascencio\Documents\dev\Hermes\Sfc"),
        ResponseExtensionType = FileExtensionType.Log
    };
}
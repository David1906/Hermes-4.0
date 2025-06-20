using Domain.Core.Types;

namespace Data.Sfc;

public class SfcApiOptions
{
    public required DirectoryInfo BaseDirectory { get; set; }
    public required FileExtensionType ResponseExtensionType { get; set; }
}
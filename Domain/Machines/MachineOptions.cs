using Domain.Core.Types;

namespace Domain.Machines;

public class MachineOptions
{
    public StationType StationName { get; set; }
    public string StationId { get; set; } = "";
    public string LineName { get; set; } = "";
    public MachineType MachineType { get; set; } = MachineType.Aoi;
    public MachineBrand MachineBrand { get; set; } = MachineBrand.Tri;
    public FileExtensionType LogFileExtension { get; set; } = FileExtensionType._3dx;
}
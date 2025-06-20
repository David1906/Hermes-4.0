namespace Data.Machines;

public class StationOptions
{
    public StationType StationName { get; set; }
    public string StationId { get; set; } = "";
    public string LineName { get; set; } = "";
    public MachineType MachineType { get; set; } = MachineType.Aoi;
    public MachineBrand MachineBrand { get; set; } = MachineBrand.Tri;
}
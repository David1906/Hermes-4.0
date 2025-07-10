namespace Core.Domain;

public class Board
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string SerialNumber { get; set; } = "";
    public List<Defect> Defects { get; init; } = [];
    public bool IsPass { get; set; }
}
using Domain.Defects;

namespace Domain.Boards;

public class Board
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = "";
    public List<Defect> Defects { get; set; } = [];
}
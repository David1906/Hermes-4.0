namespace Core.Domain;

public record Panel
{
    public int Id { get; set; }
    public string Model { get; set; } = "";
    public List<Board> Boards { get; }
    public List<Operation> Operations { get; } = [];

    public Panel(Board board)
    {
        this.Boards = [board];
    }

    public Panel(IEnumerable<Board> boards)
    {
        this.Boards = [..boards];
    }

    public string MainSerialNumber => Boards.First().SerialNumber;
    public bool ContainsFailedBoard => Boards.Any(b => !b.IsPass);
}
namespace Core.Domain;

public class Operation
{
    public int Id { get; set; }
    public required OperationType Type { get; set; }
    public OperationResultType Result { get; set; }
    public Logfile? Logfile { get; set; }
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime EndTime { get; set; } = DateTime.Now;
    public bool IsFailure => Result != OperationResultType.Pass;

    public void Start()
    {
        this.StartTime = DateTime.Now;
    }

    public void End()
    {
        this.EndTime = DateTime.Now;
    }
}
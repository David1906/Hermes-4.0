using Domain.OperationTasks;
using Domain.Panels;

namespace Domain.Operations;

public class Operation
{
    public int Id { get; set; }
    public required Panel Panel { get; init; }
    public List<OperationTask> Tasks { get; set; } = [];
    public string MainSerialNumber => Panel.MainSerialNumber;
}
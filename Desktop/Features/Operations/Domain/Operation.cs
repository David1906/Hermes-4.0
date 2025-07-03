using System.Collections.Generic;
using Desktop.Features.OperationTasks.Domain;
using Desktop.Features.Panels.Domain;

namespace Desktop.Features.Operations.Domain;

public class Operation
{
    public int Id { get; set; }
    public required Panel Panel { get; init; }
    public List<OperationTask> Tasks { get; set; } = [];
    public string MainSerialNumber => Panel.MainSerialNumber;
}
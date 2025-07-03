using Desktop.Core.Types;
using Desktop.Features.Defects.Domain;
using Desktop.Features.Logfiles.Domain;
using System.Collections.Generic;

namespace Desktop.Features.OperationTasks.Domain;

public record OperationTask
{
    public int Id { get; set; }
    public required OperationTaskType Type { get; set; }
    public required OperationTaskResultType Result { get; set; }
    public Logfile? Logfile { get; set; }
    public string Message { get; set; } = "";
    public List<Defect> Defects { get; init; } = [];
    public bool IsFailure => Result != OperationTaskResultType.Pass;
}
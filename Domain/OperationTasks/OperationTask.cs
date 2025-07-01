using Domain.Core.Types;
using Domain.Defects;
using Domain.Logfiles;

namespace Domain.OperationTasks;

public record OperationTask
{
    public int Id { get; set; }
    public required OperationTaskType Type { get; set; }
    public required OperationTaskResultType Result { get; set; }
    public string Message { get; set; } = "";
    public Logfile? Logfile { get; set; }
    public List<Defect> Defects { get; init; } = [];
}
using Desktop.Core.Types;
using Desktop.Features.Defects.Infrastructure;
using Desktop.Features.Logfiles.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Desktop.Features.OperationTasks.Infrastructure;

public class OperationTaskDbModel
{
    [Key] public int Id { get; init; }
    public required OperationTaskType Type { get; init; }
    public required OperationTaskResultType Result { get; init; }
    [MaxLength(1024)] public string Message { get; init; } = "";
    public LogfileDbModel? Logfile { get; init; }
    public List<DefectDbModel> Defects { get; init; } = [];
}
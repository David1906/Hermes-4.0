using Domain.Core.Types;
using Infrastructure.Data.Defects;
using Infrastructure.Data.Operations;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Data.Logfiles;

namespace Infrastructure.Data.OperationTasks;

public class OperationTaskDbModel
{
    [Key] public int Id { get; init; }
    public required OperationTaskType Type { get; init; }
    public required OperationTaskResultType Result { get; init; }
    [MaxLength(1024)] public string Message { get; init; } = "";
    public LogfileDbModel? Logfile { get; init; }
    public List<DefectDbModel> Defects { get; init; } = [];
}
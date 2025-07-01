using Infrastructure.Data.OperationTasks;
using Infrastructure.Data.Panels;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Operations;

public class OperationDbModel
{
    [Key] public int Id { get; set; }
    public required PanelDbModel Panel { get; init; }
    public List<OperationTaskDbModel> Tasks { get; set; } = [];
}
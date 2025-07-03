using Desktop.Features.OperationTasks.Infrastructure;
using Desktop.Features.Panels.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Desktop.Features.Operations.Infrastructure;

public class OperationDbModel
{
    [Key] public int Id { get; set; }
    public required PanelDbModel Panel { get; init; }
    public List<OperationTaskDbModel> Tasks { get; set; } = [];
}
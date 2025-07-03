using System.Collections.Generic;
using Desktop.Core.Types;
using Desktop.Features.Defects.Domain;

namespace Desktop.Features.OperationTasks.UseCases;

public record AddOperationTaskCommand(
    int OperationId,
    int LogfileId,
    OperationTaskType Type,
    OperationTaskResultType Result,
    string Message,
    List<Defect> Defects);
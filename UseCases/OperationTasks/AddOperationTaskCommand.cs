using Domain.Core.Types;
using Domain.Defects;

namespace UseCases.OperationTasks;

public record AddOperationTaskCommand(
    int OperationId,
    int LogfileId,
    OperationTaskType Type,
    OperationTaskResultType Result,
    string Message,
    List<Defect> Defects);
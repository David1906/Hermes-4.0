using Common.ResultOf;

namespace Domain.OperationTasks;

public interface IOperationTasksRepository
{
    Task<ResultOf<OperationTask>> AddAsync(int operationId, OperationTask operationTask);
}
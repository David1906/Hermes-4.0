using System.Threading.Tasks;
using Common.ResultOf;

namespace Desktop.Features.OperationTasks.Domain;

public interface IOperationTasksRepository
{
    Task<ResultOf<OperationTask>> AddAsync(int operationId, OperationTask operationTask);
}
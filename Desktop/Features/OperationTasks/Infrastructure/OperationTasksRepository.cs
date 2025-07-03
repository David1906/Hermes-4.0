using Common.ResultOf;
using Desktop.Data;
using Desktop.Features.OperationTasks.Domain;
using System.Threading.Tasks;
using System;

namespace Desktop.Features.OperationTasks.Infrastructure;

public class OperationTasksRepository(SqliteContext context) : IOperationTasksRepository
{
    public async Task<ResultOf<OperationTask>> AddAsync(int operationId, OperationTask operationTask)
    {
        try
        {
            var operation = await context.Operations.FindAsync(operationId);
            if (operation == null)
            {
                return ResultOf<OperationTask>.Failure("Operation not found.");
            }

            var operationTaskDbModel = operationTask.ToDbModel();
            context.OperationTasks.Add(operationTaskDbModel);
            await context.SaveChangesAsync();
            return operationTaskDbModel.ToDomainModel();
        }
        catch (Exception ex)
        {
            return ResultOf<OperationTask>.Failure(ex.Message);
        }
    }
}
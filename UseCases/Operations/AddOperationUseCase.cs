using Common.ResultOf;
using Domain.Operations;
using Infrastructure.Data.Operations;
using Infrastructure.Data;
using Paramore.Brighter;
using UseCases.OperationTasks;

namespace UseCases.Operations;

public class AddOperationUseCase(
    SqliteContext sqliteContext,
    IAmACommandProcessor commandProcessor
) : IUseCase<AddOperationCommand, Operation>
{
    public async Task<ResultOf<Operation>> ExecuteAsync(AddOperationCommand command, CancellationToken ct = default)
    {
        try
        {
            var dbModel = command.Operation.ToDbModel();
            sqliteContext.Operations.Add(dbModel);
            await sqliteContext.SaveChangesAsync(ct);

            await commandProcessor.PublishAsync(new OperationCreatedEvent(
                command.Operation.MainSerialNumber), cancellationToken: ct);

            foreach (var operationTask in command.Operation.Tasks)
            {
                await commandProcessor.PublishAsync(new OperationTaskCreatedEvent(
                    command.Operation.MainSerialNumber, operationTask), cancellationToken: ct);
            }


            return dbModel.ToDomainModel();
        }
        catch (Exception e)
        {
            return ResultOf<Operation>.Failure($"Failed to add operation: {e.Message}");
        }
    }
}
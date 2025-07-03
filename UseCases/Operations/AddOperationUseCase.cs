using Common.ResultOf;
using Domain.Operations;
using Infrastructure.Data.Operations;
using Infrastructure.Data;
using Paramore.Brighter;
using UseCases.OperationTasks;

namespace UseCases.Operations;

public class AddOperationUseCase(
    IOperationsRepository operationsRepository,
    IAmACommandProcessor commandProcessor
) : IUseCase<AddOperationCommand, Operation>
{
    public async Task<ResultOf<Operation>> ExecuteAsync(AddOperationCommand command, CancellationToken ct = default)
    {
        try
        {
            var result = await operationsRepository.AddAsync(command.Operation, ct);

            await commandProcessor.PublishAsync(new OperationCreatedEvent(
                command.Operation.MainSerialNumber), cancellationToken: ct);

            foreach (var operationTask in result.SuccessValue.Tasks)
            {
                await commandProcessor.PublishAsync(new OperationTaskCreatedEvent(
                    command.Operation.MainSerialNumber, operationTask), cancellationToken: ct);
            }

            return result;
        }
        catch (Exception e)
        {
            return ResultOf<Operation>.Failure($"Failed to add operation: {e.Message}");
        }
    }
}
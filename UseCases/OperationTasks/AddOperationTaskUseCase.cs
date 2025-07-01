using Common.ResultOf;
using Domain.Logfiles;
using Domain.OperationTasks;

namespace UseCases.OperationTasks;

public class AddOperationTaskUseCase(
    IOperationTasksRepository operationTasksRepository,
    ILogfilesRepository logfilesRepository)
    : IUseCase<AddOperationTaskCommand, OperationTask>
{
    public async Task<ResultOf<OperationTask>> ExecuteAsync(
        AddOperationTaskCommand command,
        CancellationToken ct = default)
    {
        try
        {
            Logfile? logfile = null;
            if (command.LogfileId > 0)
            {
                var logfileResult = await logfilesRepository.GetByIdAsync(command.LogfileId, ct);
                if (logfileResult.IsFailure)
                {
                    return ResultOf<OperationTask>.Failure(logfileResult.Errors);
                }

                logfile = logfileResult.SuccessValue;
            }

            return await operationTasksRepository.AddAsync(
                command.OperationId,
                new OperationTask()
                {
                    Type = command.Type,
                    Result = command.Result,
                    Message = command.Message,
                    Defects = command.Defects,
                    Logfile = logfile
                });
        }
        catch (Exception e)
        {
            return ResultOf<OperationTask>.Failure($"Failed to add operation: {e.Message}");
        }
    }
}
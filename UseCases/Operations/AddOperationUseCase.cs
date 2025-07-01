using Common.ResultOf;
using Domain.Operations;
using Infrastructure.Data.Operations;
using Infrastructure.Data;

namespace UseCases.Operations;

public class AddOperationUseCase(SqliteContext sqliteContext) : IUseCase<AddOperationCommand, Operation>
{
    public async Task<ResultOf<Operation>> ExecuteAsync(AddOperationCommand command, CancellationToken ct = default)
    {
        try
        {
            var dbModel = command.Operation.ToDbModel();
            sqliteContext.Operations.Add(dbModel);
            await sqliteContext.SaveChangesAsync(ct);
            return dbModel.ToDomainModel();
        }
        catch (Exception e)
        {
            return ResultOf<Operation>.Failure($"Failed to add operation: {e.Message}");
        }
    }
}
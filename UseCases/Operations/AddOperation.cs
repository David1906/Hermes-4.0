using Domain.Core.Errors;
using Domain.Operations;
using Infrastructure.Data.Operations;
using Infrastructure.Data;
using OneOf;

namespace UseCases.Operations;

public class AddOperation(SqliteContext sqliteContext) : IUseCase<AddOperationCommand, Operation>
{
    public async Task<OneOf<Operation, Error>> ExecuteAsync(AddOperationCommand command, CancellationToken ct = default)
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
            return new Error($"Failed to add operation: {e.Message}");
        }
    }
}
using Common.ResultOf;
using Domain.Operations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Operations;

public class OperationsRepository(SqliteContext sqliteContext) : IOperationsRepository
{
    public async Task<ResultOf<Operation>> AddAsync(Operation operation, CancellationToken ct)
    {
        var dbModel = operation.ToDbModel();
        sqliteContext.Operations.Add(dbModel);
        await sqliteContext.SaveChangesAsync(ct);
        return dbModel.ToDomainModel();
    }

    public async Task<ResultOf<Operation>> FindByIdAsync(int id)
    {
        var operation = await sqliteContext.Operations
            .Where(x => x.Id == id)
            .Include(x => x.Panel)
            .Include(x => x.Tasks)
            .Select(x => x.ToDomainModel())
            .FirstOrDefaultAsync();
        return operation ?? ResultOf<Operation>.Failure($"Operation with ID {id} not found.");
    }
}
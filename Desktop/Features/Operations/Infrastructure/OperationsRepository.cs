using Common.ResultOf;
using Desktop.Data;
using Desktop.Features.Operations.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Desktop.Features.Operations.Infrastructure;

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
        var operation = await Queryable
            .Where<OperationDbModel>(sqliteContext.Operations, x => x.Id == id)
            .Include(x => x.Panel)
            .Include(x => x.Tasks)
            .Select(x => x.ToDomainModel())
            .FirstOrDefaultAsync();
        return operation ?? ResultOf<Operation>.Failure($"Operation with ID {id} not found.");
    }
}
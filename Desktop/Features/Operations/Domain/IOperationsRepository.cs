using System.Threading;
using System.Threading.Tasks;
using Common.ResultOf;

namespace Desktop.Features.Operations.Domain;

public interface IOperationsRepository
{
    public Task<ResultOf<Operation>> AddAsync(Operation operation, CancellationToken ct);
    Task<ResultOf<Operation>> FindByIdAsync(int id);
}
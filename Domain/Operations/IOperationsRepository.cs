using Common.ResultOf;

namespace Domain.Operations;

public interface IOperationsRepository
{
    public Task<ResultOf<Operation>> AddAsync(Operation operation, CancellationToken ct);
}
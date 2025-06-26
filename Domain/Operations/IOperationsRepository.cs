using Domain.Core.Errors;
using OneOf;

namespace Domain.Operations;

public interface IOperationsRepository
{
    public Task<OneOf<Operation, Error>> AddAsync(Operation operation, CancellationToken ct);
}
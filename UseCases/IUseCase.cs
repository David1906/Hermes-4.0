using Domain;
using Domain.Core.Errors;
using OneOf;

namespace UseCases;

public interface IUseCase<in T, T1>
{
    public Task<OneOf<T1, Error>> ExecuteAsync(T command, CancellationToken ct = default);
}
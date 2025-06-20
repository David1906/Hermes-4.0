using ROP;

namespace UseCases;

public interface IUseCase<in T, T1>
{
    public Task<Result<T1>> ExecuteAsync(T request, CancellationToken ct = default);
}
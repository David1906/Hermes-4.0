using Common.ResultOf;

namespace UseCases;

public interface IUseCase<in T, T1>
{
    public Task<ResultOf<T1>> ExecuteAsync(T command, CancellationToken ct = default);
}
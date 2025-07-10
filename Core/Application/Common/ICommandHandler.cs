using Common.ResultOf;

namespace Core.Application.Common;

public interface ICommandHandler<in T, T1>
{
    public Task<ResultOf<T1>> ExecuteAsync(T command, CancellationToken cancellationToken = default);
}
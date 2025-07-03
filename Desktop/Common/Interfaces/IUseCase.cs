using System.Threading;
using System.Threading.Tasks;
using Common.ResultOf;

namespace Desktop.Core.Interfaces;

public interface IUseCase<in T, T1>
{
    public Task<ResultOf<T1>> ExecuteAsync(T command, CancellationToken ct = default);
}
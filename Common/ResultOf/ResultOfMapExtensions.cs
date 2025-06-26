using System.Runtime.ExceptionServices;

namespace Common.ResultOf;

public static class ResultOfMapExtensions
{
    /// <summary>
    /// Allows to get map from a result T0 to T1, the mapper method do not need to return a result T
    /// </summary>
    /// <returns>A result of type T1.</returns>
    public static ResultOf<T1> Map<T0, T1>(this ResultOf<T0> r, Func<T0, T1> mapper)
    {
        try
        {
            return r.Bind(x => ResultOf<T1>.Success(mapper(x)));
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to get map from a result T0 to T1, the mapper method do not need to return a result T
    /// </summary>
    /// <returns>A result of type T1.</returns>
    public static async Task<ResultOf<T1>> Map<T0, T1>(
        this ResultOf<T0> r,
        Func<T0, CancellationToken, Task<T1>> mapper,
        CancellationToken ct = default)
    {
        try
        {
            return await r.Bind(async (x, ct1) =>
            {
                var result = await mapper(x, ct1);
                return ResultOf<T1>.Success(result);
            }, ct);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to get map from a result T0 to T1, the mapper method do not need to return a result T
    /// </summary>
    /// <returns>A result of type T1.</returns>
    public static async Task<ResultOf<T1>> Map<T0, T1>(
        this Task<ResultOf<T0>> result,
        Func<T0, CancellationToken, Task<T1>> mapper,
        CancellationToken ct = default)
    {
        try
        {
            var r = await result;
            return await r.Map(mapper, ct);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to get map from a result T0 to T1, the mapper method do not need to return a result T
    /// </summary>
    /// <returns>A result of type T1.</returns>
    public static async Task<ResultOf<T1>> Map<T0, T1>(this Task<ResultOf<T0>> result, Func<T0, T1> mapper)
    {
        try
        {
            var r = await result;
            return r.Map(mapper);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}
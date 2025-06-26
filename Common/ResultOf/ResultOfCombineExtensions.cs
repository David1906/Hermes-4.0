using System.Runtime.ExceptionServices;

namespace Common.ResultOf;

public static class ResultOfCombineExtensions
{
    /// <summary>
    /// Allows to combine the result of two methods. the input of the combined method is the result of the first method.
    /// </summary>
    /// <returns>A result chain that contains a tuple with both results</returns>
    public static ResultOf<(T1, T2)> Combine<T1, T2>(this ResultOf<T1> resultOf, Func<T1, ResultOf<T2>> action)
    {
        try
        {
            return resultOf.Bind(action)
                .Map(x => (resultOf.AsT0, x));
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to combine the result of two methods. the input of the combined method is the result of the first method.
    /// </summary>
    /// <returns>A result chain that contains a tuple with both results</returns>
    public static async Task<ResultOf<(T1, T2)>> Combine<T1, T2>(
        this ResultOf<T1> resultOf,
        Func<T1, CancellationToken, Task<ResultOf<T2>>> action,
        CancellationToken ct = default)
    {
        try
        {
            return await resultOf.Bind(action, ct)
                .Map(x => (resultOf.AsT0, x));
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to combine the result of two methods. the input of the combined method is the result of the first method.
    /// </summary>
    /// <returns>A result chain that contains a tuple with both results</returns>
    public static async Task<ResultOf<(T1, T2)>> Combine<T1, T2>(
        this Task<ResultOf<T1>> resultOf,
        Func<T1, CancellationToken, Task<ResultOf<T2>>> action,
        CancellationToken ct = default)
    {
        try
        {
            ResultOf<T1> r = await resultOf;
            return await r.Combine(action, ct);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to combine the result of two methods. the input of the combined method is the result of the first method.
    /// </summary>
    /// <returns>A result chain that contains a tuple with both results</returns>
    public static async Task<ResultOf<(T1, T2)>> Combine<T1, T2>(
        this Task<ResultOf<T1>> resultOf,
        Func<T1, ResultOf<T2>> action)
    {
        try
        {
            ResultOf<T1> r = await resultOf;
            return r.Combine(action);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}
using System.Runtime.ExceptionServices;

namespace Common.ResultOf;

public static class ResultOfBindExtensions
{
    /// <summary>
    /// Allows to chain two methods, the output of the first is the input of the second.
    /// </summary>
    /// <param name="resultOf">current Result chain</param>
    /// <param name="method">method to execute</param>
    /// <typeparam name="T0">Input type</typeparam>
    /// <typeparam name="T1">Output type</typeparam>
    /// <returns>Result Structure of the return type</returns>
    public static ResultOf<T1> Bind<T0, T1>(this ResultOf<T0> resultOf, Func<T0, ResultOf<T1>> method)
    {
        try
        {
            return resultOf.IsSuccess
                ? method(resultOf.AsT0)
                : ResultOf<T1>.Failure(resultOf.Error);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to chain a non async method to an async method, the output of the first is the input of the second.
    /// </summary>
    /// <param name="resultOf">current Result chain</param>
    /// <param name="method">method to execute</param>
    /// <param name="ct">Cancellation token</param>
    /// <typeparam name="T0">Input type</typeparam>
    /// <typeparam name="T1">Output type</typeparam>
    /// <returns>Async Result Structure of the return type</returns>
    public static async Task<ResultOf<T1>> Bind<T0, T1>(
        this ResultOf<T0> resultOf,
        Func<T0, CancellationToken, Task<ResultOf<T1>>> method,
        CancellationToken ct = default)
    {
        try
        {
            return resultOf.IsSuccess
                ? await method(resultOf.AsT0, ct)
                : ResultOf<T1>.Failure(resultOf.Error);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to chain two async methods, the output of the first is the input of the second.
    /// </summary>
    /// <param name="resultOf">current Result chain</param>
    /// <param name="method">method to execute</param>
    /// <param name="ct">Cancellation token</param>
    /// <typeparam name="T0">Input type</typeparam>
    /// <typeparam name="T1">Output type</typeparam>
    /// <returns>Async Result Structure of the return type</returns>
    public static async Task<ResultOf<T1>> Bind<T0, T1>(
        this Task<ResultOf<T0>> resultOf,
        Func<T0, CancellationToken, Task<ResultOf<T1>>> method,
        CancellationToken ct = default)
    {
        try
        {
            var r = await resultOf;
            return await r.Bind(method, ct);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to chain an async method to a non async method, the output of the first is the input of the second.
    /// </summary>
    /// <param name="resultOf">current Result chain</param>
    /// <param name="method">method to execute</param>
    /// <typeparam name="T0">Input type</typeparam>
    /// <typeparam name="T1">Output type</typeparam>
    /// <returns>Async Result Structure of the return type</returns>
    public static async Task<ResultOf<T1>> Bind<T0, T1>(
        this Task<ResultOf<T0>> resultOf,
        Func<T0, ResultOf<T1>> method)
    {
        try
        {
            var r = await resultOf;
            return r.Bind(method);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}
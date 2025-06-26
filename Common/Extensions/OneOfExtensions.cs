using System.Runtime.ExceptionServices;
using OneOf;

namespace Common.Extensions;

public static class OneOfExtensions
{
    /// <summary>
    /// Allows to chain two methods, the output of the first is the input of the second.
    /// </summary>
    /// <param name="r">current Result chain</param>
    /// <param name="method">method to execute</param>
    /// <typeparam name="TInput">Input type</typeparam>
    /// <typeparam name="TOutput0">Output type 0</typeparam>
    /// <typeparam name="TOutput1">Output type 1</typeparam>
    /// <returns>Result Structure of the return type</returns>
    public static OneOf<TOutput1, TOutput0> Bind<TInput, TOutput0, TOutput1>(
        this OneOf<TInput, TOutput0> r,
        Func<TInput, OneOf<TOutput1, TOutput0>> method)
    {
        try
        {
            return r.IsT0
                ? method(r.AsT0)
                : r.AsT1;
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to chain two methods, the output of the first is the input of the second.
    /// </summary>
    /// <param name="r">current Result chain</param>
    /// <param name="method">method to execute</param>
    /// <typeparam name="TInput">Input type</typeparam>
    /// <typeparam name="TOutput0">Output type 0</typeparam>
    /// <typeparam name="TOutput1">Output type 1</typeparam>
    /// <returns>Result Structure of the return type</returns>
    public static async Task<OneOf<TOutput1, TOutput0>> Bind<TInput, TOutput0, TOutput1>(
        this OneOf<TInput, TOutput0> r,
        Func<TInput, Task<OneOf<TOutput1, TOutput0>>> method)
    {
        try
        {
            return r.IsT0
                ? await method(r.AsT0)
                : r.AsT1;
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to chain two methods, the output of the first is the input of the second.
    /// </summary>
    /// <param name="r">current Result chain</param>
    /// <param name="method">method to execute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TInput">Input type</typeparam>
    /// <typeparam name="TOutput0">Output type 0</typeparam>
    /// <typeparam name="TOutput1">Output type 1</typeparam>
    /// <returns>Result Structure of the return type</returns>
    public static async Task<OneOf<TOutput1, TOutput0>> Bind<TInput, TOutput0, TOutput1>(
        this OneOf<TInput, TOutput0> r,
        Func<TInput, CancellationToken, Task<OneOf<TOutput1, TOutput0>>> method,
        CancellationToken cancellationToken)
    {
        try
        {
            return r.IsT0
                ? await method(r.AsT0, cancellationToken)
                : r.AsT1;
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to chain two methods, the output of the first is the input of the second.
    /// </summary>
    /// <param name="r">current Result chain</param>
    /// <param name="method">method to execute</param>
    /// <typeparam name="TInput">Input type</typeparam>
    /// <typeparam name="TOutput0">Output type 0</typeparam>
    /// <typeparam name="TOutput1">Output type 1</typeparam>
    /// <returns>Result Structure of the return type</returns>
    public static async Task<OneOf<TOutput1, TOutput0>> Bind<TInput, TOutput0, TOutput1>(
        this Task<OneOf<TInput, TOutput0>> r,
        Func<TInput, Task<OneOf<TOutput1, TOutput0>>> method)
    {
        try
        {
            var result = await r;
            return result.IsT0
                ? await method(result.AsT0)
                : result.AsT1;
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>
    /// Allows to chain two methods, the output of the first is the input of the second.
    /// </summary>
    /// <param name="r">current Result chain</param>
    /// <param name="method">method to execute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TInput">Input type</typeparam>
    /// <typeparam name="TOutput0">Output type 0</typeparam>
    /// <typeparam name="TOutput1">Output type 1</typeparam>
    /// <returns>Result Structure of the return type</returns>
    public static async Task<OneOf<TOutput1, TOutput0>> Bind<TInput, TOutput0, TOutput1>(
        this Task<OneOf<TInput, TOutput0>> r,
        Func<TInput, CancellationToken, Task<OneOf<TOutput1, TOutput0>>> method,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await r;
            return result.IsT0
                ? await method(result.AsT0, cancellationToken)
                : result.AsT1;
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}
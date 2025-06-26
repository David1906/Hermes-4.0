using System.Runtime.ExceptionServices;
using OneOf;

namespace Common.Extensions;

public static class OneOfCombineExtensions
{
    /// <summary>
    /// Allows to combine the result of two methods. the input of the combined method is the result of the first method.
    /// </summary>
    /// <returns>A result chain that contains a tuple with both results</returns>
    public static OneOf<(TInput, TOutput1), TOutput0> Combine<TInput, TOutput0, TOutput1>(
        this OneOf<TInput, TOutput0> r,
        Func<TInput, OneOf<TOutput1, TOutput0>> method)
    {
        try
        {
            return r.Bind(method)
                .MapT0(x => (r.AsT0, x));
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
    public static async Task<OneOf<(TInput, TOutput1), TOutput0>> Combine<TInput, TOutput0, TOutput1>(
        this Task<OneOf<TInput, TOutput0>> r,
        Func<TInput, Task<OneOf<TOutput1, TOutput0>>> method)
    {
        try
        {
            var r1 = await r;
            var a = await r1.Bind(method);
            return a.MapT0(x => (r1.AsT0, x));
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}
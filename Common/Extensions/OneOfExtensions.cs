using System.Runtime.ExceptionServices;
using OneOf;

namespace Common.Extensions;

public static class OneOfExtensions
{
    /// <summary>
    /// Allows to chain two methods, the output of the first is the input of the second.
    /// </summary>
    /// <param name="r">current OneOf chain</param>
    /// <param name="method">method to execute</param>
    /// <typeparam name="T0">Type 0</typeparam>
    /// <typeparam name="T1">Type 1</typeparam>
    /// <returns>One of the return types</returns>
    public static OneOf<T0, T1> Bind<T0, T1>(this OneOf<T0, T1> r, Func<T0, OneOf<T0, T1>> method)
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
    /// <param name="r">current OneOf chain</param>
    /// <param name="method">method to execute</param>
    /// <typeparam name="T0">Type 0</typeparam>
    /// <typeparam name="T1">Type 1</typeparam>
    /// <returns>One of the return types</returns>
    public static async Task<OneOf<T0, T1>> Bind<T0, T1>(this Task<OneOf<T0, T1>> r, Func<T0, Task<OneOf<T0, T1>>> method)
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
}
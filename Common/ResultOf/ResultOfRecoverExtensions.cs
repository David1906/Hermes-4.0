using System.Collections.Immutable;

namespace Common.ResultOf;

public static class ResultOfRecoverExtensions
{
    /// <summary>
    /// Attempts to recover from a failure by applying the provided recovery function.
    /// </summary>
    /// <returns>A result of type T.</returns>
    public static async Task<ResultOf<T>> Recover<T>(
        this Task<ResultOf<T>> result,
        Func<ResultOf<T>, IEnumerable<Error>, Task<ResultOf<T>>> recoveryFunction)
    {
        var r = await result;
        if (r.IsFailure)
        {
            return await recoveryFunction(r, r.Errors);
        }

        return r;
    }

    /// <summary>
    /// Attempts to recover from a failure by applying the provided recovery function.
    /// </summary>
    /// <returns>A result of type T.</returns>
    public static ResultOf<T> Recover<T>(
        this ResultOf<T> result,
        Func<ResultOf<T>, IEnumerable<Error>, ResultOf<T>> recoveryFunction)
    {
        if (result.IsFailure)
        {
            return recoveryFunction(result, result.Errors);
        }

        return result;
    }
}
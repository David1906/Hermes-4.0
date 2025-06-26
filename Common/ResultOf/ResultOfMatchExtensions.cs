using System.Collections.Immutable;

namespace Common.ResultOf;

public static class ResultOfMatchExtensions
{
    /// <summary>
    /// Executes the provided actions based on the result type.
    /// </summary>
    public static async Task<TResult> Match<T, TResult>(
        this Task<ResultOf<T>> result,
        Func<T, Task<TResult>> onSuccess,
        Func<ImmutableArray<Error>, Task<TResult>> onFailure)
    {
        var r = await result;
        return await r.Match(
            onSuccess,
            onFailure
        );
    }
}
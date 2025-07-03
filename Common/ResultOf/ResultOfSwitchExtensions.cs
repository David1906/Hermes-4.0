using System.Collections.Immutable;

namespace Common.ResultOf;

public static class ResultOfSwitchExtensions
{
    /// <summary>
    /// Executes the provided actions based on the result type.
    /// </summary>
    public static async Task<ResultOf<T>> Switch<T>(
        this Task<ResultOf<T>> result,
        Action<T> onSuccess,
        Action<ImmutableArray<Error>> onFailure)
    {
        var r = await result;
        r.Switch(
            onSuccess,
            onFailure
        );
        return r;
    }

    /// <summary>
    /// Executes the provided action if the result is successful.
    /// </summary>
    public static ResultOf<T> OnSuccess<T>(
        this ResultOf<T> result,
        Action<T> onSuccess)
    {
        if (result.IsSuccess)
        {
            onSuccess(result.AsT0);
        }

        return result;
    }

    /// <summary>
    /// Executes the provided action if the result is successful.
    /// </summary>
    public static async Task<ResultOf<T>> OnSuccess<T>(
        this Task<ResultOf<T>> result,
        Func<T, Task> onSuccess)
    {
        var r = await result;
        if (r.IsSuccess)
        {
            await onSuccess(r.AsT0);
        }

        return r;
    }

    /// <summary>
    /// Executes the provided action if the result is successful.
    /// </summary>
    public static async Task<ResultOf<T>> OnSuccess<T>(
        this Task<ResultOf<T>> result,
        Action<T> onSuccess)
    {
        var r = await result;
        return r.OnSuccess(onSuccess);
    }

    /// <summary>
    /// Executes the provided action if the result is failure.
    /// </summary>
    public static async Task<ResultOf<T>> OnFailure<T>(
        this Task<ResultOf<T>> result,
        Func<ImmutableArray<Error>, Task> onFailure)
    {
        var r = await result;
        if (!r.IsSuccess)
        {
            await onFailure(r.AsT1);
        }

        return r;
    }

    /// <summary>
    /// Executes the provided action if the result is failure.
    /// </summary>
    public static ResultOf<T> OnFailure<T>(
        this ResultOf<T> result,
        Action<ImmutableArray<Error>> onFailure)
    {
        if (!result.IsSuccess)
        {
            onFailure(result.AsT1);
        }

        return result;
    }

    /// <summary>
    /// Executes the provided action if the result is failure.
    /// </summary>
    public static async Task<ResultOf<T>> OnFailure<T>(
        this Task<ResultOf<T>> result,
        Action<ImmutableArray<Error>> onFailure)
    {
        var r = await result;
        return r.OnFailure(onFailure);
    }
}
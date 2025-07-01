namespace Common.ResultOf;

public static class ResultOfTapExtensions
{
    /// <summary>
    /// Executes a final action after the result of a task is completed, regardless of its success or failure.
    /// </summary>
    /// <returns>A result of type T.</returns>
    public static async Task<ResultOf<T>> Tap<T>(
        this Task<ResultOf<T>> resultTask,
        Func<ResultOf<T>, Task> actionAsync)
    {
        var result = await resultTask;
        await actionAsync(result);
        return result;
    }

    /// <summary>
    /// Executes a final action after the result of a task is completed, regardless of its success or failure.
    /// </summary>
    /// <returns>A result of type T.</returns>
    public static async Task<ResultOf<T>> Tap<T>(
        this Task<ResultOf<T>> resultTask,
        Func<ResultOf<T>, Task<ResultOf<T>>> actionAsync)
    {
        var result = await resultTask;
        await actionAsync(result);
        return result;
    }

    /// <summary>
    /// Executes a final action after the result of a task is completed, regardless of its success or failure.
    /// </summary>
    /// <returns>A result of type T.</returns>
    public static ResultOf<T> Tap<T>(
        this ResultOf<T> result,
        Action<ResultOf<T>> action)
    {
        action(result);
        return result;
    }
}
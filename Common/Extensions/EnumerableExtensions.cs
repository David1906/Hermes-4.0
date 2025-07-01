namespace Common.Extensions;

public static class EnumerableExtensions
{
    public static string JoinWithNewLine<T>(this IEnumerable<T> values) where T : class
    {
        return string.Join("\n", values.Select(x => x.ToString()));
    }
}
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using OneOf;

namespace Common.ResultOf;

public class ResultOf<T> : OneOfBase<T, ImmutableArray<Error>>
{
    public bool IsSuccess => this.IsT0;
    public bool IsFailure => !this.IsSuccess;

    public IEnumerable<Error> Errors => this.IsT1
        ? this.AsT1
        : ImmutableArray<Error>.Empty;


    /// <summary>
    /// Implicitly converts a value of type T to a successful result with an HTTP status code of OK.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator ResultOf<T>(T value) => new(value);

    public ResultOf(OneOf<T, ImmutableArray<Error>> input) : base(input)
    {
    }

    public static ResultOf<T> Failure(string error)
    {
        return Failure(new Error(error));
    }

    public static ResultOf<T> Failure(Error error)
    {
        return new ResultOf<T>(ImmutableArray.Create(error));
    }

    public static ResultOf<T> Failure(IEnumerable<Error> errors)
    {
        return new ResultOf<T>(ImmutableArray.CreateRange(errors));
    }

    public static ResultOf<T> Success(T value)
    {
        return new ResultOf<T>(value);
    }
}
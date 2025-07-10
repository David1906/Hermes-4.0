using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using OneOf;

namespace Common.ResultOf;

public class ResultOf<T> : OneOfBase<T, Error>
{
    public bool IsSuccess => this.IsT0;
    public bool IsFailure => !this.IsSuccess;
    public Error Error => this.AsT1;

    public new T Value => this.AsT0;

    /// <summary>
    /// Implicitly converts a value of type T to a successful result with an HTTP status code of OK.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator ResultOf<T>(T value) => new(value);

    public ResultOf(OneOf<T, Error> error) : base(error)
    {
    }

    public static ResultOf<T> Failure(string error)
    {
        return Failure(new UnknownError(error));
    }

    public static ResultOf<T> Failure(Error error)
    {
        return new ResultOf<T>(error);
    }

    public static ResultOf<T> Success(T value)
    {
        return new ResultOf<T>(value);
    }
}
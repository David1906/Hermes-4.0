using Common.ResultOf;

namespace Core.Domain.Common.Errors;

public class UnknownError : Error
{
    public UnknownError()
        : base("Unknown error")
    {
    }

    public UnknownError(string message)
        : base(message)
    {
    }
}
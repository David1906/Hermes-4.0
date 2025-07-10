using Common.ResultOf;

namespace Core.Application.Common.Errors;

public class InvalidDataError(string message)
    : Error(message)
{
    public InvalidDataError()
        : this("Invalid data")
    {
    }
}
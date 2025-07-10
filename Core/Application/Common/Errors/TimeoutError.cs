using Common.ResultOf;

namespace Core.Application.Common.Errors;

public class TimeoutError(string message)
    : Error(message)
{
    public TimeoutError()
        : this("Timeout")
    {
    }
}
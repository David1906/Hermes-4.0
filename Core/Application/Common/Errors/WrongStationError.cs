using Common.ResultOf;

namespace Core.Application.Common.Errors;

public class WrongStationError(string message)
    : Error(message)
{
    public WrongStationError()
        : this("Wrong station")
    {
    }
}
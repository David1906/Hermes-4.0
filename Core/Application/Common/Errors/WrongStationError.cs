using Common.Languages;
using Common.ResultOf;

namespace Core.Application.Common.Errors;

public class WrongStationError(string message)
    : Error(message)
{
    public WrongStationError()
        : this("Wrong station")
    {
    }

    public override string TranslatedErrorType => Resources.msg_wrong_station_error;
}
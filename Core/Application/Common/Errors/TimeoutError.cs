using Common.Languages;
using Common.ResultOf;

namespace Core.Application.Common.Errors;

public class TimeoutError(string message)
    : Error(message)
{
    public TimeoutError()
        : this("Timeout")
    {
    }

    public override string TranslatedErrorType => Resources.msg_timeout_error;
}
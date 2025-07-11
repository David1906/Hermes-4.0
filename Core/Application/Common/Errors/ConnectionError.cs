using Common.Languages;
using Common.ResultOf;

namespace Core.Application.Common.Errors;

public class ConnectionError(string message)
    : Error(message)
{
    public ConnectionError()
        : this("Connection error")
    {
    }

    public override string TranslatedErrorType => Resources.msg_connection_error;
}
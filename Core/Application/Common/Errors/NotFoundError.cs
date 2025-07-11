using Common.Languages;
using Common.ResultOf;

namespace Core.Application.Common.Errors;

public class NotFoundError(string message)
    : Error(message)
{
    public NotFoundError()
        : this("Not found")
    {
    }

    public override string TranslatedErrorType => Resources.msg_not_found_error;
}
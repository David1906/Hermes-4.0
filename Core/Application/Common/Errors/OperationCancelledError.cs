using Common.Languages;
using Common.ResultOf;

namespace Core.Application.Common.Errors;

public class OperationCancelledError(string message)
    : Error(message)
{
    public OperationCancelledError()
        : this("Operation cancelled")
    {
    }

    public override string TranslatedErrorType => Resources.msg_operation_cancelled_error;
}
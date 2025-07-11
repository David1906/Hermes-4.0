using Common.Languages;
using Common.ResultOf;

namespace Core.Application.Common.Errors;

public class ScanningError(string message)
    : Error(message)
{
    public ScanningError()
        : this("Scanning error")
    {
    }

    public override string TranslatedErrorType => Resources.msg_scanning_error;
}
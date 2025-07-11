using Common.Languages;
using Common.ResultOf;

namespace Core.Application.Common.Errors;

public class EndOfFileError(string message)
    : Error(message)
{
    public EndOfFileError()
        : this("End of file")
    {
    }

    public override string TranslatedErrorType => Resources.msg_end_of_file_error;
}
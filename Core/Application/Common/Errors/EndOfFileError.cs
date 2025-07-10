using Common.ResultOf;

namespace Core.Application.Common.Errors;

public class EndOfFileError(string message)
    : Error(message)
{
    public EndOfFileError()
        : this("End of file")
    {
    }
}
namespace Common.ResultOf;

public class UnknownError : Error
{
    public UnknownError()
        : base("Unknown error")
    {
    }

    public UnknownError(string message)
        : base(message)
    {
    }
}
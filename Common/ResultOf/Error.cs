namespace Common.ResultOf;

public class Error
{
    public string Message { get; }
    public string ErrorType { get; }

    public Error(string message)
    {
        Message = message;
        ErrorType = GetType().Name;
    }

    public override string ToString()
    {
        return $"{ErrorType}: {Message}";
    }
}
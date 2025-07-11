namespace Common.ResultOf;

public abstract class Error
{
    public int Id { get; set; }
    public string Message { get; }
    public string ErrorType { get; }
    public abstract string TranslatedErrorType { get; }

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
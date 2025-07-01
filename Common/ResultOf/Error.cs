namespace Common.ResultOf;

public class Error
{
    public static readonly Error Unknown = new Error(1000, "Unknown error");
    public static readonly Error Ok = new Error(1001, "Ok");
    public static readonly Error Fail = new Error(1002, "Fail");
    public static readonly Error Timeout = new Error(1003, "Timeout");
    public static readonly Error Repair = new Error(1004, "Repair");
    public static readonly Error WrongStation = new Error(1005, "Wrong station");
    public static readonly Error OperationCancelled = new Error(1006, "Operation cancelled");
    public static readonly Error EndOfFile = new Error(1007, "End of file");
    public static readonly Error ConnectionError = new Error(1008, "Connection error");
    public static readonly Error UploadSerialNumbersError = new Error(1009, "Upload serial numbers error");
    public static readonly Error ScanningError = new Error(1010, "Scanning error");

    public int Id { get; }
    public string Message { get; }

    public Error(string message) : this(Unknown.Id, message)
    {
    }

    public Error() : this(Unknown.Id, Unknown.Message)
    {
    }

    public Error(int id, string message)
    {
        Id = id;
        Message = message;
    }

    public override string ToString()
    {
        return $"{Id}: {Message}";
    }
}
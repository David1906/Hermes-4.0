namespace Desktop.Core.Types;

public enum UploadResultType
{
    Unknown,
    Ok,
    Fail,
    Timeout,
    Repair,
    WrongStation,
    Exception,
    EndOfFile,
    ConnectionError,
    UploadSerialNumbersError,
    SerialNumberNotRead
}
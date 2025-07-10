namespace Core.Domain;

public enum OperationResultType
{
    Unknown,
    Pass,
    Fail,
    Warning,
    Error,
    NotApplicable,
    InProgress,
    Skipped,
    WrongStation,
    EndOfFile,
    TimedOut
}
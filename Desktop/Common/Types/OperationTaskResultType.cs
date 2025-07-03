namespace Desktop.Core.Types;

public enum OperationTaskResultType
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
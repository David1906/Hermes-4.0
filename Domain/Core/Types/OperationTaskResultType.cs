namespace Domain.Core.Types;

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
    EndOfFile
}
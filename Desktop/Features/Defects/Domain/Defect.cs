using Desktop.Core.Types;

namespace Desktop.Features.Defects.Domain;

public class Defect
{
    public int Id { get; init; }
    public bool IsRealDefect { get; init; }
    public ErrorFlagType ErrorFlag { get; init; } = ErrorFlagType.Good;
    public string Location { get; init; } = "";
    public string ErrorCode { get; init; } = "";
}
using Domain.Core.Types;

namespace Domain.Defects;

public class Defect
{
    public static readonly Defect Null = new DefectNull();

    public int Id { get; init; }
    public bool IsRealDefect { get; init; }
    public ErrorFlagType ErrorFlag { get; init; } = ErrorFlagType.Good;
    public string Location { get; init; } = "";
    public string ErrorCode { get; init; } = "";
    public bool IsNull => this == Null;
}

public class DefectNull : Defect
{
}
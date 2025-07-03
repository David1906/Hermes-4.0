using System.ComponentModel.DataAnnotations;
using Desktop.Core.Types;

namespace Desktop.Features.Defects.Infrastructure;

public class DefectDbModel
{
    [Key] public int Id { get; init; }
    public bool IsRealDefect { get; init; }
    public ErrorFlagType ErrorFlag { get; init; } = ErrorFlagType.Good;
    [StringLength(124)] public string Location { get; init; } = "";
    [StringLength(124)] public string ErrorCode { get; init; } = "";
}
using System.ComponentModel.DataAnnotations;
using Core.Domain;
using Core.Domain.Common.Types;

namespace Infrastructure.Data.Features.Defects;

public class DefectDto
{
    public int Id { get; set; }
    public ErrorFlagType ErrorFlag { get; set; } = ErrorFlagType.Good;
    [MaxLength(255)] public string Location { get; set; } = "";
    [MaxLength(255)] public string ErrorCode { get; set; } = "";
}
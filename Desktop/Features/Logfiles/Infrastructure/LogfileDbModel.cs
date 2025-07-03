using Desktop.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace Desktop.Features.Logfiles.Infrastructure;

public class LogfileDbModel
{
    [Key] public int Id { get; init; }
    public LogfileType Type { get; set; }
    [MaxLength(512)] public string FileInfo { get; set; } = string.Empty;
}
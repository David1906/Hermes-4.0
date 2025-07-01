using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Logfiles;

public class LogfileDbModel
{
    [Key] public int Id { get; init; }
    [MaxLength(512)] public string FileInfo { get; set; } = string.Empty;
}
using Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Features.Logfiles;

public class LogfileDbModel
{
    public int Id { get; set; }
    [MaxLength(255)] public string FileInfo { get; set; } = "";
    public LogfileType Type { get; set; }
}
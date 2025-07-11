using Core.Domain;
using System.ComponentModel.DataAnnotations;
using Core.Domain.Common.Types;

namespace Infrastructure.Data.Features.Logfiles;

public class LogfileDto
{
    public int Id { get; set; }
    [MaxLength(255)] public string FileInfo { get; set; } = "";
    public LogfileType Type { get; set; }
}
using Core.Domain;
using Infrastructure.Data.Features.Errors;
using Infrastructure.Data.Features.Logfiles;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Features.Operations;

public class OperationDto
{
    [Key] public int Id { get; set; }
    public OperationType Type { get; set; }
    public ErrorDto? Error { get; set; }
    public LogfileDto? Logfile { get; set; }
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime EndTime { get; set; } = DateTime.Now;
}
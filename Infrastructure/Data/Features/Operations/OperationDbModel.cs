using Core.Domain;
using Infrastructure.Data.Features.Errors;
using Infrastructure.Data.Features.Logfiles;

namespace Infrastructure.Data.Features.Operations;

public class OperationDbModel
{
    public int Id { get; set; }
    public required OperationType Type { get; set; }
    public required ErrorDbModel Error { get; set; }
    public LogfileDbModel? Logfile { get; set; }
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime EndTime { get; set; } = DateTime.Now;
}
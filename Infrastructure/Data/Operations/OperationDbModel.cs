using Domain.Core.Types;
using Infrastructure.Data.Boards;
using Infrastructure.Data.Logfiles;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Operations;

public class OperationDbModel
{
    [Key] public int Id { get; set; }
    public List<BoardDbModel> Boards { get; set; } = [];
    public OperationResultType Result { get; set; } = OperationResultType.Unknown;
    public LogfileDbModel? Logfile { get; set; }
    public UploadResultType UploadResult { get; set; } = UploadResultType.Unknown;
    public LogfileDbModel? UploadLogfile { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool SkipForSampling { get; set; }
    public bool IsAutoSend { get; set; }
    public bool IsManuallyRemove { get; set; }
}
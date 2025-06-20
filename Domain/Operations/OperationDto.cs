using Domain.Boards;
using Domain.Core.Types;
using Domain.Logfiles;

namespace Domain.Operations;

public class OperationDto
{
    public int Id { get; set; }
    public List<Board> Boards { get; set; } = [];
    public OperationResultType Result { get; set; } = OperationResultType.Unknown;
    public Logfile? Logfile { get; set; }
    public UploadResultType UploadResult { get; set; } = UploadResultType.Unknown;
    public Logfile? UploadResponse { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool SkipForSampling { get; set; }
    public bool IsAutoSend { get; set; }
    public bool IsManuallyRemove { get; set; }
}
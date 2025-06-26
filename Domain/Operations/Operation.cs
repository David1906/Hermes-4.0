using Domain.Boards;
using Domain.Core.Types;
using Domain.Logfiles;

namespace Domain.Operations;

public class Operation
{
    public int Id { get; set; }
    public List<Board> Boards { get; set; } = [];
    public OperationResultType Result { get; set; } = OperationResultType.Unknown;
    public Logfile? Logfile { get; set; }
    public UploadResultType UploadResult { get; set; } = UploadResultType.Unknown;
    public Logfile? UploadLogfile { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool SkipForSampling { get; set; }
    public bool IsAutoSend { get; set; }
    public bool IsManuallyRemove { get; set; }
    public string MainSerialNumber => Boards.FirstOrDefault()?.SerialNumber ?? string.Empty;
}
using Common.ResultOf;
using Core.Domain.Common.Types;

namespace Core.Domain;

public class Operation
{
    public int Id { get; set; }
    public required OperationType Type { get; set; }
    public Error? Error { get; set; }
    public Logfile? Logfile { get; set; }
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime EndTime { get; set; } = DateTime.Now;

    public bool IsFailure => Error is not null;
    public bool IsPass => !IsFailure;

    public string Title => this.Error?.Message ?? Type.ToString();
    public string TranslatedErrorType => this.Error?.TranslatedErrorType ?? "";

    public void Start()
    {
        this.StartTime = DateTime.Now;
    }

    public void End()
    {
        this.EndTime = DateTime.Now;
    }

    public void End(Error error)
    {
        this.Error = error;
        this.End();
    }
}
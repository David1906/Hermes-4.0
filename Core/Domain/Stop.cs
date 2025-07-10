using Common.ResultOf;

namespace Core.Domain;

public class Stop
{
    public string Title { get; set; } = "Stop";
    public string SerialNumber { get; set; } = "";
    public string Actions { get; set; } = "";
    public List<Error> Errors { get; init; } = [];
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime? EndTime { get; set; }
}
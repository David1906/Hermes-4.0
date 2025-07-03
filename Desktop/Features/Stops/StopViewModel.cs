using Desktop.Core;
using Domain.OperationTasks;

namespace Desktop.Features.Locking;

public class StopViewModel : ViewModelBase
{
    public string Title { get; set; } = "Stop";
    public string SerialNumber { get; set; } = "";
    public string Actions { get; set; } = "";
}
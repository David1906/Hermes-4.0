using System;
using Desktop.Common;
using Desktop.Core;

namespace Desktop.Features.Operations.Delivery;

public class SuccessViewModel : ViewModelBase
{
    public bool IsRepair { get; set; }
    public string SerialNumber { get; set; } = "";
    public string Message { get; set; } = "";
    public TimeSpan CloseAfter { get; set; } = TimeSpan.FromSeconds(5);
}
using System;
using UI.Common;

namespace UI.Features.Panels;

public class SuccessViewModel : ViewModelBase
{
    public bool IsRepair { get; set; }
    public string SerialNumber { get; set; } = "";
    public string Message { get; set; } = "";
    public TimeSpan CloseAfter { get; set; } = TimeSpan.FromSeconds(5);
}
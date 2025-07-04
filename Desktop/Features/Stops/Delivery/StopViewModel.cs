using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Desktop.Common;
using Desktop.Core;
using R3;

namespace Desktop.Features.Stops.Delivery;

public partial class StopViewModel : ViewModelBase
{
    public string Title { get; set; } = "Stop";
    public string SerialNumber { get; set; } = "";
    public string Actions { get; set; } = "";
    public DateTime StartTime { get; set; } = DateTime.Now;

    [ObservableProperty] private string _elapsedTime = "00:00:00";

    public StopViewModel()
    {
        this.SetupRx();
    }

    private void SetupRx()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => this.ElapsedTime = $"{DateTime.Now - this.StartTime:hh\\:mm\\:ss}")
            .AddTo(ref Disposables);
    }
}
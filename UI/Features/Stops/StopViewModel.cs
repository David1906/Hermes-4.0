using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Domain.Common.Types;
using R3;
using UI.Common;

namespace UI.Features.Stops;

public partial class StopViewModel : ViewModelBase
{
    public string Title { get; set; } = "Stop";
    public string ErrorType { get; set; } = "";
    public string SerialNumber { get; set; } = "";
    public string Actions { get; set; } = "";
    public DateTime StartTime { get; set; } = DateTime.Now;
    public List<DepartmentType> Departments { get; set; } = [];

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
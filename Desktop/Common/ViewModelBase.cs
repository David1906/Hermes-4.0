using System;
using CommunityToolkit.Mvvm.ComponentModel;
using R3;

namespace Desktop.Common;

public class ViewModelBase : ObservableObject, IDisposable
{
    protected DisposableBag Disposables;

    public void Dispose()
    {
        Disposables.Dispose();
    }
}
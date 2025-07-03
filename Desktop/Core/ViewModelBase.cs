using CommunityToolkit.Mvvm.ComponentModel;
using R3;

namespace Desktop.Core;

public class ViewModelBase : ObservableObject
{
    protected DisposableBag Disposables;
}
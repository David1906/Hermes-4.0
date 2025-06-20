using CommunityToolkit.Mvvm.ComponentModel;
using R3;

namespace Desktop.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected DisposableBag Disposables;
}
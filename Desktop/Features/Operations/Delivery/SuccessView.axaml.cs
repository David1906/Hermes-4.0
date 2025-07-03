using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Desktop.Features.Operations.Delivery;

public partial class SuccessView : Window
{
    public SuccessView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        this.SetBottomCenterPosition();

        base.OnLoaded(e);
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        if (DataContext is SuccessViewModel vm)
        {
            Task.Delay(vm.CloseAfter).ContinueWith(_ => { this.Close(); },
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        base.OnDataContextChanged(e);
    }

    private void SetBottomCenterPosition()
    {
        var screenSize = Screens.Primary!.WorkingArea.Size;
        var windowSize = PixelSize.FromSize(ClientSize, Screens.Primary.Scaling);

        Position = new PixelPoint(
            screenSize.Width / 2 - windowSize.Width / 2,
            screenSize.Height - windowSize.Height);
    }

    private void InputElement_OnTapped(object? sender, TappedEventArgs e)
    {
        this.Close();
    }
}
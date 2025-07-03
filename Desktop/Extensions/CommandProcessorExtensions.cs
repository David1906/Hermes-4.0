using Avalonia.Controls.Notifications;
using Desktop.Core;
using Desktop.Events;
using Paramore.Brighter;

namespace Desktop.Extensions;

public static class CommandProcessorExtensions
{
    public static void ShowSuccessToast(
        this IAmACommandProcessor processor,
        string message,
        bool autoDismiss = true)
    {
        ShowToastAsync(
            processor,
            message,
            NotificationType.Success,
            autoDismiss
        );
    }

    public static void ShowErrorToast(
        this IAmACommandProcessor processor,
        string message,
        bool autoDismiss = true)
    {
        ShowToastAsync(
            processor,
            message,
            NotificationType.Error,
            autoDismiss
        );
    }

    public static void ShowInformationToast(
        this IAmACommandProcessor processor,
        string message,
        bool autoDismiss = true)
    {
        ShowToastAsync(
            processor,
            message,
            NotificationType.Information,
            autoDismiss
        );
    }

    private static void ShowToastAsync(
        IAmACommandProcessor processor,
        string message,
        NotificationType toastType,
        bool autoDismiss = true)
    {
        processor.Publish(new ShowToastEvent()
        {
            Title = toastType.ToString(),
            Message = message,
            ToastType = toastType,
            AutoDismiss = autoDismiss
        });
    }

    public static void OpenWindow(
        this IAmACommandProcessor processor,
        ViewModelBase viewModel)
    {
        processor.Publish(new OpenWindowEvent()
        {
            ViewModel = viewModel
        });
    }
}
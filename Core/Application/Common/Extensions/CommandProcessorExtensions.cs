using Core.Application.Common.Events;
using Core.Application.Common.Types;
using Paramore.Brighter;

namespace Core.Application.Common.Extensions;

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
}
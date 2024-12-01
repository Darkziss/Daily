using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;

namespace Daily.Toasts
{
    public static class ToastCreator
    {
        public const ToastDuration toastDuration = ToastDuration.Long;
        public const double toastTextSize = 16d;

        public static IToast Create(string message)
        {
            return Toast.Make(message, toastDuration, toastTextSize);
        }
    }
}
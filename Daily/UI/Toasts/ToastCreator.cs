using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;

namespace Daily.Toasts
{
    public static class ToastCreator
    {
        private const double ToastTextSize = 16d;

        public static IToast CreateShortByTime(string message) => Toast.Make(message, ToastDuration.Short, ToastTextSize);

        public static IToast CreateLongByTime(string message) => Toast.Make(message, ToastDuration.Long, ToastTextSize);
    }
}
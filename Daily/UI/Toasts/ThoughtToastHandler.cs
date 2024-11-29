using CommunityToolkit.Maui.Core;

namespace Daily.Toasts
{
    public class ThoughtToastHandler
    {
        private static readonly IToast _thoughtCreatedToast = ToastCreator.Create(thoughtCreatedMessage);

        private static readonly IToast _thoughtErrorToast = ToastCreator.Create(thoughtErrorMessage);

        private const string thoughtCreatedMessage = "Мысль была успешно создана";

        private const string thoughtErrorMessage = "Ошибка\nПопробуйте еще раз"; 

        public static async Task ShowThoughtCreatedToastAsync() => await _thoughtCreatedToast.Show();

        public static async Task ShowThoughtErrorToastAsync() => await _thoughtErrorToast.Show();
    }
}
using CommunityToolkit.Maui.Core;

namespace Daily.Toasts
{
    public static class GoalToastHandler
    {
        private static readonly IToast deadlineDateErrorToast = ToastCreator.CreateLongByTime(deadlineDateErrorMessage);

        private const string deadlineDateErrorMessage = "Выберите будущую дату";

        public static async Task ShowDeadlineDateErrorToastAsync() => await deadlineDateErrorToast.Show();
    }
}
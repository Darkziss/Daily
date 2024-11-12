using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;

namespace Daily.Toasts
{
    public static class TaskToastHandler
    {
        private static readonly IToast _taskCreatedToast = Toast.Make(taskCreatedToastMessage, 
            toastDuration, toastTextSize);
        private static readonly IToast _taskEditedToast = Toast.Make(taskEditedToastMessage,
            toastDuration, toastTextSize);

        private static readonly IToast _generalTasksFullToast = Toast.Make(generalTasksFullToastMessage,
            toastDuration, toastTextSize);
        private static readonly IToast _conditionalTasksFullToast = Toast.Make(conditionalTasksFullToastMessage,
            toastDuration, toastTextSize);

        private const string taskCreatedToastMessage = "Задача была успешно создана";
        private const string taskEditedToastMessage = "Задача была успешно изменена";

        private const string generalTasksFullToastMessage = "Ошибка: Уже создано максимум основных задач";
        private const string conditionalTasksFullToastMessage = "Ошибка: Уже создано максимум условных задач";

        private const ToastDuration toastDuration = ToastDuration.Long;
        private const double toastTextSize = 16d;

        public static async Task ShowTaskCreatedToastAsync() => await _taskCreatedToast.Show();

        public static async Task ShowTaskEditedToastAsync() => await _taskEditedToast.Show();

        public static async Task ShowGeneralTasksFullToastAsync() => await _generalTasksFullToast.Show();

        public static async Task ShowConditionalTasksFullToastAsync() => await _conditionalTasksFullToast.Show();
    }
}

using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;

namespace Daily.Toasts
{
    public static class TaskToastHandler
    {
        private static readonly IToast _taskCreatedToast = ToastCreator.Create(taskCreatedToastMessage);
        private static readonly IToast _taskEditedToast = ToastCreator.Create(taskEditedToastMessage);
        private static readonly IToast _taskDeletedToast = ToastCreator.Create(taskDeletedToastMessage);

        private static readonly IToast _taskErrorToast = ToastCreator.Create(taskErrorToastMessage);

        private static readonly IToast _oneTimeTasksFullToast = ToastCreator.Create(oneTimeTasksFullToastMessage);
        private static readonly IToast _recurringTasksFullToast = ToastCreator.Create(recurringTasksFullToastMessage);

        private const string taskCreatedToastMessage = "Задача была успешно создана";
        private const string taskEditedToastMessage = "Задача была успешно изменена";
        private const string taskDeletedToastMessage = "Задача удалена";

        private const string taskErrorToastMessage = "Ошибка\nПопробуйте еще раз";

        private const string oneTimeTasksFullToastMessage = "Ошибка: Уже создано максимум разовых задач";
        private const string recurringTasksFullToastMessage = "Ошибка: Уже создано максимум цикличных задач";

        public static async Task ShowTaskCreatedToastAsync() => await _taskCreatedToast.Show();

        public static async Task ShowTaskEditedToastAsync() => await _taskEditedToast.Show();

        public static async Task ShowTaskDeletedToastAsync() => await _taskDeletedToast.Show();

        public static async Task ShowTaskErrorToastAsync() => await _taskErrorToast.Show();

        public static async Task ShowOneTimeTasksFullToastAsync() => await _oneTimeTasksFullToast.Show();

        public static async Task ShowRecurringTasksFullToastAsync() => await _recurringTasksFullToast.Show();
    }
}

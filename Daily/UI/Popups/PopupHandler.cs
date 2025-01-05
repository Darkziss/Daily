
namespace Daily.Popups
{
    public static class PopupHandler
    {
        private const string accept = "Да";
        private const string cancel = "Нет";
        
        public static async Task<bool> ShowTaskDeletePopupAsync()
        {
            const string title = "Удаление задачи";
            const string message = "Вы хотите удалить задачу?";

            return await ShowDialogPopupAtCurrentPageAsync(title, message, accept, cancel);
        }

        public static async Task<bool> ShowThoughtDeletePopupAsync()
        {
            const string title = "Удаление мысли";
            const string message = "Вы хотите удалить мысль?";

            return await ShowDialogPopupAtCurrentPageAsync(title, message, accept, cancel);
        }

        public static async Task<bool> ShowDiaryRecordDeletePopupAsync()
        {
            const string title = "Удаление записи";
            const string message = "Вы хотите удалить запись?";

            return await ShowDialogPopupAtCurrentPageAsync(title, message, accept, cancel);
        }
        
        public static async Task<bool> ShowRecordExitPopupAsync()
        {
            const string title = "Выход";
            const string message = "У вас есть несохраненные изменения. Вы уверены, что хотите выйти?";

            return await ShowDialogPopupAtCurrentPageAsync(title, message, accept, cancel);
        }

        private static async Task<bool> ShowDialogPopupAtCurrentPageAsync(string title, string message, string accept, string cancel)
        {
            Page page = Shell.Current.CurrentPage;

            return await page.DisplayAlert(title, message, accept, cancel);
        }
    }
}
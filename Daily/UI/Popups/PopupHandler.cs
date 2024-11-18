
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

            return await ShowPopupAtCurrentPageAsync(title, message, accept, cancel);
        }
        
        private static async Task<bool> ShowPopupAtCurrentPageAsync(string title, string message, string accept, string cancel)
        {
            Page page = Shell.Current.CurrentPage;

            return await page.DisplayAlert(title, message, accept, cancel);
        }
    }
}

namespace Daily.Popups
{
    public static class PopupHandler
    {
        public static async Task<bool> ShowPopupAtCurrentPage(string title, string message, string accept, string cancel)
        {
            Page page = Shell.Current.CurrentPage;

            return await page.DisplayAlert(title, message, accept, cancel);
        }
    }
}
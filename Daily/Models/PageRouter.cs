
namespace Daily
{
    public static class PageRouter
    {
        private static bool _isRouting = false;

        public static bool IsRouting => _isRouting;

        private const bool animateRouting = false;

        public static async Task RouteTo(string pageName)
        {
            if (_isRouting || string.IsNullOrWhiteSpace(pageName)) return;

            _isRouting = true;

            await Shell.Current.GoToAsync(pageName, animateRouting);

            _isRouting = false;
        }
    }
}
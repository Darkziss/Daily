
namespace Daily
{
    public static class PageRouter
    {
        private static bool _isRouting = false;

        public static bool IsRouting => _isRouting;

        private const string backwards = "..";
        private const bool animateRouting = false;

        private const string routingExceptionText = "Already routing to page";
        private const string nullPageNameExceptionText = "Page name is null";

        public static async Task RouteToPage(string pageName)
        {
            if (_isRouting) throw new Exception(routingExceptionText);
            else if (string.IsNullOrWhiteSpace(pageName)) throw new ArgumentNullException(nullPageNameExceptionText);

            _isRouting = true;

            await Shell.Current.GoToAsync(pageName, animateRouting);

            _isRouting = false;
        }

        public static async Task RouteToPreviousPage() => await RouteToPage(backwards);
    }
}
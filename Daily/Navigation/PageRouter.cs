
namespace Daily
{
    public static class PageRouter
    {
        private static bool _isRouting = false;

        public static bool IsRouting => _isRouting;

        private const string backwards = "..";
        private const bool animateRouting = false;

        private const string routingExceptionText = "Already routing to page";

        public static async Task RouteTo(string pageName)
        {
            if (_isRouting || string.IsNullOrWhiteSpace(pageName)) throw new Exception(routingExceptionText);

            _isRouting = true;

            await Shell.Current.GoToAsync(pageName, animateRouting);

            _isRouting = false;
        }

        public static async Task RouteToPrevious()
        {
            if (_isRouting) throw new Exception(routingExceptionText);

            _isRouting = true;

            await Shell.Current.GoToAsync(backwards, animateRouting);

            _isRouting = false;
        }
    }
}
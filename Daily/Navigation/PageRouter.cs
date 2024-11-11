
namespace Daily
{
    public static class PageRouter
    {
        public static bool IsRouting { get; private set; }

        private const string backwards = "..";
        private const bool animateRouting = true;

        private const string routingExceptionText = "Already routing to page";
        private const string invalidPageNameExceptionText = "Page name is null or white space";
        private const string nullNavigationParameterExceptionText = "Navigation parameters are null";

        public static async Task RouteToPage(string pageName)
        {
            if (IsRouting) throw new Exception(routingExceptionText);
            else if (string.IsNullOrWhiteSpace(pageName)) throw new ArgumentException(invalidPageNameExceptionText);

            IsRouting = true;

            await Shell.Current.GoToAsync(pageName, animateRouting);

            IsRouting = false;
        }

        public static async Task RouteToPageWithParameters(string pageName, ShellNavigationQueryParameters parameters)
        {
            if (IsRouting) throw new Exception(routingExceptionText);
            else if (string.IsNullOrWhiteSpace(pageName)) throw new ArgumentException(invalidPageNameExceptionText);
            else if (parameters == null) throw new ArgumentNullException(nullNavigationParameterExceptionText);

            IsRouting = true;

            await Shell.Current.GoToAsync(pageName, animateRouting, parameters);

            IsRouting = false;
        }

        public static async Task RouteToPreviousPage() => await RouteToPage(backwards);
    }
}
using Daily.Pages;

namespace Daily.Navigation
{
    public static class PageNavigator
    {
        public static bool IsRouting { get; private set; }

        private const string backwards = "..";
        private const bool animateRouting = true;

        private const string routingExceptionText = "Already routing to page";
        private const string invalidPageNameExceptionText = "Page name is null or white space";
        private const string nullNavigationParameterExceptionText = "Navigation parameters are null";

        public static async Task GoToTaskPageAsync() => await RouteToPage(nameof(TaskPage));

        public static async Task GoToTaskEditPageAsync() => await RouteToPage(nameof(TaskEditPage));

        public static async Task GoToTaskEditPageWithParametersAsync(ShellNavigationQueryParameters parameters) => await RouteToPageWithParameters(nameof(TaskEditPage), parameters);

        public static async Task GoToThoughtPageAsync() => await RouteToPage(nameof(ThoughtPage));

        public static async Task GoToThoughtEditPageAsync() => await RouteToPage(nameof(ThoughtEditPage));

        public static async Task RouteToPreviousPage() => await RouteToPage(backwards);

        private static async Task RouteToPage(string pageName)
        {
            if (IsRouting) throw new Exception(routingExceptionText);
            else if (string.IsNullOrWhiteSpace(pageName)) throw new ArgumentException(invalidPageNameExceptionText);

            IsRouting = true;

            await Shell.Current.GoToAsync(pageName, animateRouting);

            IsRouting = false;
        }

        private static async Task RouteToPageWithParameters(string pageName, ShellNavigationQueryParameters parameters)
        {
            if (IsRouting) throw new Exception(routingExceptionText);
            else if (string.IsNullOrWhiteSpace(pageName)) throw new ArgumentException(invalidPageNameExceptionText);
            else if (parameters == null) throw new ArgumentNullException(nullNavigationParameterExceptionText);

            IsRouting = true;

            await Shell.Current.GoToAsync(pageName, animateRouting, parameters);

            IsRouting = false;
        }
    }
}
using Daily.Pages;

namespace Daily.Navigation
{
    public static class PageNavigator
    {
        public static bool IsRouting { get; private set; }

        private const string backwards = "..";
        private const bool animateRouting = true;

        private const string routingExceptionText = "Already routing to page";
        private const string nullNavigationParameterExceptionText = "Navigation parameters are null";

        #region Tasks

        public static async Task GoToTaskPageAsync() => await RouteToPage(nameof(TaskPage));

        public static async Task GoToTaskEditPageAsync() => await RouteToPage(nameof(TaskEditPage));

        public static async Task GoToTaskEditPageWithParametersAsync(ShellNavigationQueryParameters parameters) => await RouteToPageWithParameters(nameof(TaskEditPage), parameters);

        #endregion

        #region Thoughts

        public static async Task GoToThoughtPageAsync() => await RouteToPage(nameof(ThoughtPage));

        public static async Task GoToThoughtEditPageAsync() => await RouteToPage(nameof(ThoughtEditPage));

        public static async Task GoToThoughtEditPageWithParametersAsync(ShellNavigationQueryParameters parameters) => await RouteToPageWithParameters(nameof(ThoughtEditPage), parameters);

        #endregion

        #region Diary

        public static async Task GoToDiaryRecordPageAsync() => await RouteToPage(nameof(DiaryRecordPage));

        public static async Task GoToDiaryRecordEditPageAsync() => await RouteToPage(nameof(DiaryRecordEditPage));

        public static async Task GoToDiaryRecordEditPageWithParametersAsync(ShellNavigationQueryParameters parameters) => await RouteToPageWithParameters(nameof(DiaryRecordEditPage), parameters);

        #endregion

        public static async Task ReturnToPreviousPage() => await RouteToPage(backwards);

        private static async Task RouteToPage(string pageName)
        {
            if (IsRouting) throw new Exception(routingExceptionText);

            IsRouting = true;

            await Shell.Current.GoToAsync(pageName, animateRouting);

            IsRouting = false;
        }

        private static async Task RouteToPageWithParameters(string pageName, ShellNavigationQueryParameters parameters)
        {
            if (IsRouting) throw new Exception(routingExceptionText);
            else if (parameters == null) throw new ArgumentNullException(nullNavigationParameterExceptionText);

            IsRouting = true;

            await Shell.Current.GoToAsync(pageName, animateRouting, parameters);

            IsRouting = false;
        }
    }
}
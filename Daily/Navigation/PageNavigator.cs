using Daily.Pages;

namespace Daily.Navigation
{
    public static class PageNavigator
    {
        public static bool IsRouting { get; private set; }

        private const string backwards = "..";
        private const bool animate = true;

        private const string routingExceptionText = "Already routing to page";

        public static async Task GoToTaskPageAsync() => await GoToPageAsync(nameof(TaskPage));

        public static async Task GoToTaskEditPageAsync(ShellNavigationQueryParameters? parameters = null)
        {
            await GoToPageAsync(nameof(TaskEditPage), parameters);
        }

        public static async Task GoToGoalEditPageAsync() => await GoToPageAsync(nameof(GoalEditPage));

        public static async Task GoToThoughtPageAsync() => await GoToPageAsync(nameof(ThoughtPage));

        public static async Task GoToThoughtEditPageAsync(ShellNavigationQueryParameters? parameters = null)
        {
            await GoToPageAsync(nameof(ThoughtEditPage), parameters);
        }

        public static async Task GoToDiaryRecordPageAsync() => await GoToPageAsync(nameof(DiaryRecordPage));

        public static async Task GoToDiaryRecordEditPageAsync(ShellNavigationQueryParameters? parameters = null)
        {
            await GoToPageAsync(nameof(DiaryRecordEditPage), parameters);
        }

        public static async Task ReturnToPreviousPageAsync() => await GoToPageAsync(backwards);

        private static async Task GoToPageAsync(string pageName, ShellNavigationQueryParameters? parameters = null)
        {
            if (IsRouting) throw new Exception(routingExceptionText);

            IsRouting = true;

            if (parameters == null) await Shell.Current.GoToAsync(pageName, animate);
            else await Shell.Current.GoToAsync(pageName, animate, parameters);

            IsRouting = false;
        }
    }
}
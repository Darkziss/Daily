using Daily.Sheets;

namespace Daily.Navigation
{
    public static class SheetNavigator
    {
        public static bool IsShowing { get; private set; }

        public static bool IsHiding { get; private set; }
        
        private const bool animate = true;

        private const string exceptionText = "Already showing/hiding sheet";

        public static async Task ShowGoalSheetAsync() => await ShowSheetAsync(nameof(GoalBottomSheet));

        public static async Task HideCurrentSheetAsync()
        {
            if (IsHiding) throw new Exception(exceptionText);

            IsHiding = true;

            await SheetShell.HideCurrentSheetAsync(animate);

            IsHiding = false;
        }
        
        private static async Task ShowSheetAsync(string sheetName)
        {
            if (IsShowing) throw new Exception(exceptionText);

            IsShowing = true;

            await SheetShell.ShowSheetAsync(sheetName, animate);

            IsShowing = false;
        }
    }
}
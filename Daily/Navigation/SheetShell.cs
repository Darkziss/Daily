using The49.Maui.BottomSheet;

namespace Daily.Navigation
{
    public static class SheetShell
    {
        private static BottomSheet? _currentSheet;
        
        private static readonly Dictionary<string, BottomSheet> _routes = new Dictionary<string, BottomSheet>();

        private static bool IsShowingAnySheet => _currentSheet != null;

        public static void RegisterRoute(string route, Type type)
        {
            if (!ValidateRoute(route)) throw new ArgumentNullException();

            BottomSheet sheet = (BottomSheet)Activator.CreateInstance(type)!;

            _routes.Add(route, sheet);
        }

        public static void UnregisterRoute(string route)
        {
            bool removed = _routes.Remove(route);

            if (!removed) throw new ArgumentException();
        }

        public static async Task ShowSheetAsync(string sheetName, bool animate)
        {
            if (IsShowingAnySheet) throw new Exception();
            
            BottomSheet sheet = _routes[sheetName];

            _currentSheet = sheet;

            await sheet.ShowAsync(animate);
        }

        public static async Task HideCurrentSheetAsync(bool animate)
        {
            if (!IsShowingAnySheet) throw new Exception();

            BottomSheet sheet = _currentSheet!;
            _currentSheet = null;

            await sheet.DismissAsync(animate);
        }

        private static bool ValidateRoute(string route) => !string.IsNullOrWhiteSpace(route);
    }
}
using CommunityToolkit.Mvvm.ComponentModel;

namespace Daily.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private bool _isRouting = false;
        
        public Command RouteToTaskPage { get; private set; }

        public MainPageViewModel()
        {
            RouteToTaskPage = new Command(
            execute: async () =>
            {
                await RouteTo(nameof(TaskPage));
            },
            canExecute: () =>
            {
                return !_isRouting;
            });
        }

        private async Task RouteTo(string pageName)
        {
            _isRouting = true;

            await Shell.Current.GoToAsync(pageName);

            _isRouting = false;
        }
    }
}
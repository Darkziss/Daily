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
            execute: () =>
            {
                RouteTo(nameof(TaskPage));
                System.Diagnostics.Debug.WriteLine("Routing...");
            },
            canExecute: () =>
            {
                return !_isRouting;
            });
        }

        private async void RouteTo(string pageName)
        {
            _isRouting = true;

            await Shell.Current.GoToAsync(pageName);
        }
    }
}
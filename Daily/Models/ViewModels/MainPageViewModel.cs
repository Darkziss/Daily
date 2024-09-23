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
                await PageRouter.RouteTo(nameof(TaskPage));
            },
            canExecute: () =>
            {
                return !PageRouter.IsRouting;
            });
        }
    }
}
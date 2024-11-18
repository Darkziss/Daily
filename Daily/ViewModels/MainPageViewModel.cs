using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Navigation;

namespace Daily.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, IResetView
    {
        public Command RouteToTaskPage { get; }

        public MainPageViewModel()
        {
            RouteToTaskPage = new Command(
            execute: async () =>
            {
                await PageNavigator.GoToTaskPageAsync();
            },
            canExecute: () =>
            {
                return !PageNavigator.IsRouting;
            });
        }

        public void ResetView()
        {
            RouteToTaskPage.ChangeCanExecute();
        }
    }
}
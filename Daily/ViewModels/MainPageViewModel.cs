using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Pages;

namespace Daily.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, IPrepareView
    {
        public Command RouteToTaskPage { get; }
        public Command RouteToTaskEditPage { get; }

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

            RouteToTaskEditPage = new Command(
            execute: async () =>
            {
                await PageRouter.RouteTo(nameof(TaskEditPage));
            },
            canExecute: () =>
            {
                return !PageRouter.IsRouting;
            });
        }

        public void PrepareView()
        {
            RouteToTaskPage.ChangeCanExecute();
            RouteToTaskEditPage.ChangeCanExecute();
        }
    }
}
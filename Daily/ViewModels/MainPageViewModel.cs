using CommunityToolkit.Mvvm.ComponentModel;

namespace Daily.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, IPrepareView
    {
        public Command RouteToTaskPage { get; }

        public MainPageViewModel()
        {
            RouteToTaskPage = new Command(
            execute: async () =>
            {
                await PageRouter.RouteToPage(nameof(TaskPage));
            },
            canExecute: () =>
            {
                return !PageRouter.IsRouting;
            });
        }

        public void PrepareView()
        {
            RouteToTaskPage.ChangeCanExecute();
        }
    }
}
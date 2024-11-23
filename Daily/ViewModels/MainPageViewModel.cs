using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Navigation;

namespace Daily.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private bool _canNavigate = false;
        
        public Command GoToTaskPage { get; }

        public MainPageViewModel()
        {
            GoToTaskPage = new Command(
            execute: async () =>
            {
                CanNavigate = false;

                await PageNavigator.GoToTaskPageAsync();

                CanNavigate = true;
            });
        }

        public void ResetView()
        {
            CanNavigate = true;
        }
    }
}
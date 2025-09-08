using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Tasks;
using Daily.Navigation;

namespace Daily.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _canNavigate = true;

        public Command GoToTaskPage { get; }
        public Command GoToThoughtPage { get; }
        public Command GoToDiaryRecordPage { get; }

        public string CurrentVersion => GetCurrentVersion();

        public MainPageViewModel()
        {
            GoToTaskPage = new Command(
            execute: async () =>
            {
                CanNavigate = false;

                await PageNavigator.GoToTaskPageAsync();
            });

            GoToThoughtPage = new Command(
            execute: async () =>
            {
                CanNavigate = false;

                await PageNavigator.GoToThoughtPageAsync();
            });

            GoToDiaryRecordPage = new Command(
            execute: async () =>
            {
                CanNavigate = false;

                await PageNavigator.GoToDiaryRecordPageAsync();
            });
        }

        public void MakeViewReady()
        {
            CanNavigate = true;
        }

        private string GetCurrentVersion()
        {
            string version = AppInfo.VersionString;

#if DEBUG
            return $"v{version} (debug)";
#else

            return $"v{version}";
#endif
        }
    }
}
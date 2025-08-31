using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Tasks;
using Daily.Navigation;

namespace Daily.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private bool _isCounterVisible = true;

        [ObservableProperty] private string _dailyCounterText = string.Empty;
        [ObservableProperty] private string _mandatoryCounterText = string.Empty;
        [ObservableProperty] private string _importantCounterText = string.Empty;
        [ObservableProperty] private string _commonCounterText = string.Empty;

        [ObservableProperty] private string _dummyText = string.Empty;

        [ObservableProperty] private bool _canNavigate = true;

        public Command GoToTaskPage { get; }
        public Command GoToThoughtPage { get; }
        public Command GoToDiaryRecordPage { get; }

        public string CurrentVersion => GetCurrentVersion();

        private const string emptyStatusText = "Задач нет";
        private const string completedStatusText = "Все задачи выполнены!";

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

        public void ResetView()
        {
            //RefreshTaskProgressStatus();
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
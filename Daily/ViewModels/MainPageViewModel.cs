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

        private readonly IReadOnlyList<GeneralTask> _generalTasks;

        public Command GoToTaskPage { get; }
        public Command GoToThoughtPage { get; }
        public Command GoToDiaryRecordPage { get; }

        public string CurrentVersion => GetCurrentVersion();

        private const string emptyStatusText = "Задач нет";
        private const string completedStatusText = "Все задачи выполнены!";

        public MainPageViewModel(TaskStorage taskStorage)
        {
            _generalTasks = taskStorage.GeneralTasks;
            
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
            RefreshTaskProgressStatus();
        }

        private void RefreshTaskProgressStatus()
        {
            bool isEmpty = _generalTasks.Count == 0;

            if (isEmpty)
            {
                DummyText = emptyStatusText;
                IsCounterVisible = false;
                return;
            }

            bool isAllCompleted = _generalTasks.All((task) => task.IsCompleted);

            if (isAllCompleted)
            {
                DummyText = completedStatusText;
                IsCounterVisible = false;
                return;
            }

            Span<int> priorityCounts = stackalloc int[4];

            for (int i = 0; i < _generalTasks.Count; i++)
            {
                var task = _generalTasks[i];
                int index = (int)task.Priority;

                if (!task.IsCompleted) priorityCounts[index]++;
            }

            DailyCounterText = priorityCounts[0].ToString();
            MandatoryCounterText = priorityCounts[1].ToString();
            ImportantCounterText = priorityCounts[2].ToString();
            CommonCounterText = priorityCounts[3].ToString();

            IsCounterVisible = true;
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
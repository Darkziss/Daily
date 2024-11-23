using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Tasks;
using Daily.Navigation;

namespace Daily.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private string _statusText = defaultStatusText;
        [ObservableProperty] private Color _statusColor = Colors.Transparent;

        [ObservableProperty] private bool _canNavigate = false;

        private readonly TaskStorage _taskStorage;

        private static readonly Color dailyColor = Color.FromRgba("edd8c3");
        private static readonly Color mandatoryColor = Color.FromRgba("edc3c3");
        private static readonly Color importantColor = Color.FromRgba("edebc3");
        private static readonly Color commonColor = Color.FromRgba("c3edc4");

        private static readonly Color emptyColor = Color.FromRgba("858585");
        private static readonly Color completedColor = Color.FromRgba("71b866");

        public Command GoToTaskPage { get; }

        private const string defaultStatusText = "Загрузка..";
        private const string emptyStatusText = "Задач нет";
        private const string allDoneStatusText = "Все задачи выполнены!";

        public MainPageViewModel(TaskStorage taskStorage)
        {
            _taskStorage = taskStorage;
            
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
            StatusText = defaultStatusText;
            StatusColor = Colors.Transparent;

            CanNavigate = true;

            SetTaskProgressStatusText();
        }

        private void SetTaskProgressStatusText()
        {
            IReadOnlyList<GeneralTask> tasks = _taskStorage.GeneralTasks;
            bool isEmpty = tasks.Count == 0;

            if (isEmpty)
            {
                StatusText = emptyStatusText;
                StatusColor = emptyColor;
                return;
            }

            Span<int> priorityCounts = stackalloc int[4];
            Span<bool> tasksLeft = stackalloc bool[4];

            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                int index = (int)task.Priority;

                if (!task.IsCompleted) priorityCounts[index]++;

                tasksLeft[index] = priorityCounts[index] > 0;
            }

            if (tasksLeft[1])
            {
                int mandatoryCount = priorityCounts[1];

                StatusText = mandatoryCount == 1 ? $"Осталась 1 обязательная задача"
                    : $"Осталось {mandatoryCount} обязательных задач";
                StatusColor = mandatoryColor;
            }
            else if (tasksLeft[2])
            {
                int importantCount = priorityCounts[2];

                StatusText = importantCount == 1 ? $"Осталась 1 важная задача"
                    : $"Осталось {importantCount} важных задач";
                StatusColor = importantColor;
            }
            else if (tasksLeft[0])
            {
                int dailyCount = priorityCounts[0];

                StatusText = dailyCount == 1 ? $"Осталась 1 ежедневная задача"
                    : $"Осталось {dailyCount} ежедневных задач";
                StatusColor = dailyColor;
            }
            else if (tasksLeft[3])
            {
                StatusText = "Выполнены все задачи, кроме обычных";
                StatusColor = completedColor;
            }
            else
            {
                StatusText = allDoneStatusText;
                StatusColor = completedColor;
            }
        }
    }
}
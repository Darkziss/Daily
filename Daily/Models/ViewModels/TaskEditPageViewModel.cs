using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Tasks;

namespace Daily.ViewModels
{
    public partial class TaskEditPageViewModel : ObservableObject
    {
        private readonly TaskStorage _taskStorage;

        [ObservableProperty] private bool _isCreatingNewTask = false;

        public string TaskAction { get; set; } = string.Empty;
        public int SelectedPriorityIndex { get; set; } = 0;
        public int RepeatCount { get; set; } = 1;

        public Command CreateGeneralTaskCommand { get; }

        private const int loadingDelay = 1500;

        public TaskEditPageViewModel(TaskStorage taskStorage)
        {
            _taskStorage = taskStorage;

            CreateGeneralTaskCommand = new Command(
            execute: async () =>
            {
                IsCreatingNewTask = true;
                
                TaskPriority priority = (TaskPriority)SelectedPriorityIndex;

#if DEBUG
                Debug.WriteLine($"Creating a new task: {TaskAction}");
#endif
                await taskStorage.CreateGeneralTaskAsync(TaskAction,
                    priority, RepeatCount);

                await Task.Delay(loadingDelay);

                IsCreatingNewTask = false;
            },
            canExecute: () => !_isCreatingNewTask);
        }
    }
}
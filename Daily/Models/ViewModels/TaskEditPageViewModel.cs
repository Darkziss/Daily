using System.Diagnostics;
using Daily.Tasks;

namespace Daily.ViewModels
{
    public class TaskEditPageViewModel
    {
        private readonly TaskStorage _taskStorage;

        public string TaskAction { get; set; } = string.Empty;
        public int SelectedPriorityIndex { get; set; } = 0;
        public int RepeatCount { get; set; } = 1;

        public Command CreateTaskCommand { get; }

        public TaskEditPageViewModel(TaskStorage taskStorage)
        {
            _taskStorage = taskStorage;

            CreateTaskCommand = new Command(
            execute: async () =>
            {
                TaskPriority priority = (TaskPriority)SelectedPriorityIndex;

                await taskStorage.CreateGeneralTaskAsync(TaskAction,
                    priority, RepeatCount);
            });
        }
    }
}
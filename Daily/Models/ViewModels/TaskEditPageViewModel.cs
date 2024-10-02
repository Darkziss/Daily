using System.Diagnostics;
using Daily.Tasks;

namespace Daily.ViewModels
{
    public class TaskEditPageViewModel
    {
        private string _taskAction = string.Empty;
        private TaskPriority _taskPriority = TaskPriority.Daily;
        private int _repeatCount = 1;

        private readonly TaskStorage _taskStorage;

        public string TaskAction { get => _taskAction; set => _taskAction = value; }
        public int SelectedPriorityIndex { get => (int)_taskPriority; set => _taskPriority = (TaskPriority)value; }
        public int RepeatCount { get => _repeatCount; set => _repeatCount = value; }

        public Command CreateTaskCommand { get; }

        public TaskEditPageViewModel(TaskStorage taskStorage)
        {
            _taskStorage = taskStorage;

            CreateTaskCommand = new Command(
            execute: async () => await taskStorage.CreateGeneralTaskAsync(_taskAction,
                _taskPriority, _repeatCount));
        }
    }
}
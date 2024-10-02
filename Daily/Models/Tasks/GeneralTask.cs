
namespace Daily.Tasks
{
    public struct GeneralTask
    {
        private string _taskAction = string.Empty;
        private TaskPriority _priority = TaskPriority.Daily;
        private int _repeatCount = 1;

        private bool _isCompleted = false;

        public string TaskAction => _taskAction;
        public TaskPriority Priority => _priority;
        public int RepeatCount => _repeatCount;

        public bool IsCompleted { get => _isCompleted; set => _isCompleted = value; }

        public GeneralTask(string taskAction, TaskPriority priority, int repeatCount)
        {
            _taskAction = taskAction;
            _priority = priority;
            _repeatCount = repeatCount;
        }
    }
}
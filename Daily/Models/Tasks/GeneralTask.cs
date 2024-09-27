
namespace Daily.Tasks
{
    public class GeneralTask
    {
        private string _taskAction = string.Empty;
        private TaskPriority _priority = TaskPriority.Daily;
        private int _repeatCount = 1;

        private bool _isCompleted = false;

        public string TaskAction => _taskAction;
        public TaskPriority Priority => _priority;
        public int RepeatCount => _repeatCount;

        public bool IsCompleted => _isCompleted;
    }
}
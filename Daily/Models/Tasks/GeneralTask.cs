
namespace Daily.Tasks
{
    public struct GeneralTask
    {
        public string TaskAction { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; } = TaskPriority.Daily;
        public int RepeatCount { get; set; } = 1;

        public bool IsCompleted { get; set; } = false;

        public GeneralTask(string taskAction, TaskPriority priority, int repeatCount)
        {
            TaskAction = taskAction;
            Priority = priority;
            RepeatCount = repeatCount;
        }
    }
}
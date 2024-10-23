
namespace Daily.Tasks
{
    public struct GeneralTask
    {
        public string ActionName { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; } = TaskPriority.Daily;
        public int RepeatCount { get; set; } = 0;
        public int TargetRepeatCount { get; set; } = 1;

        public bool IsCompleted { get; set; } = false;

        public GeneralTask(string actionName, TaskPriority priority, int targetRepeatCount)
        {
            ActionName = actionName;
            Priority = priority;
            TargetRepeatCount = targetRepeatCount;
        }
    }
}
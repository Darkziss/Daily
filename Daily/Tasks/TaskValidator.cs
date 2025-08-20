
namespace Daily.Tasks
{
    public static class TaskValidator
    {
        private const int minRepeatCount = 1;
        private const int maxRepeatCount = 10;

        public static bool ValidateTask(TaskBase task)
        {
            if (string.IsNullOrWhiteSpace(task.ActionName)) return false;
            else if (task.TargetRepeatCount < minRepeatCount || task.TargetRepeatCount > maxRepeatCount) return false;
            else return true;
        }
    }
}
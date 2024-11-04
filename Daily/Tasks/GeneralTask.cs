using System.ComponentModel;

namespace Daily.Tasks
{
    public class GeneralTask : TaskBase
    {
        private TaskPriority _priority;

        public TaskPriority Priority
        {
            get => _priority;
            set
            {
                if (value == _priority) return;

                _priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }

        public GeneralTask(string actionName, TaskPriority priority, int targetRepeatCount) : base(actionName, targetRepeatCount)
        {
            Priority = priority;
        }
    }
}
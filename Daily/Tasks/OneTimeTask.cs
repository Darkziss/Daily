using System.ComponentModel;

namespace Daily.Tasks
{
    public class OneTimeTask : TaskBase
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

        public OneTimeTask(string actionName, int repeatCount, int targetRepeatCount, TaskPriority priority, string note) 
            : base(actionName, repeatCount, targetRepeatCount, note)
        {
            Priority = priority;
        }
    }
}
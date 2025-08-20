
namespace Daily.Tasks
{
    public class ConditionalTask : TaskBase
    {
        private TaskRepeatTimePeriod _repeatTimePeriod;

        public TaskRepeatTimePeriod RepeatTimePeriod
        {
            get => _repeatTimePeriod;
            set
            {
                if (value == _repeatTimePeriod) return;

                _repeatTimePeriod = value;
                OnPropertyChanged(nameof(RepeatTimePeriod));
            }
        }

        public ConditionalTask(string actionName, int repeatCount, int targetRepeatCount, 
            TaskRepeatTimePeriod repeatTimePeriod, string note) 
            : base(actionName, repeatCount, targetRepeatCount, note)
        {
            RepeatTimePeriod = repeatTimePeriod;
        }
    }
}
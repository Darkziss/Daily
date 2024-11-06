
namespace Daily.Tasks
{
    public class СonditionalTask : TaskBase
    {
        private TaskRepeatTimePeriod _repeatTimePeriod;

        private int _completionTime = 0;

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

        public int CompletionTime
        {
            get => _completionTime;
            set
            {
                if (value == _completionTime) return;

                _completionTime = value;
                OnPropertyChanged(nameof(CompletionTime));
            }
        }

        public СonditionalTask(string actionName, int targetRepeatCount, TaskRepeatTimePeriod repeatTimePeriod, int completionTime) : base(actionName, targetRepeatCount)
        {
            RepeatTimePeriod = repeatTimePeriod;
            CompletionTime = completionTime;
        }
    }
}
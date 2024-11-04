
namespace Daily.Tasks
{
    public class СonditionalTask : TaskBase
    {
        private TimeSpan _repeatTimePeriod;

        private int _completionTime = 0;

        public TimeSpan RepeatTimePeriod
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

        public СonditionalTask(string actionName, int targetRepeatCount, TimeSpan repeatTimePeriod, int minCompletionTimeMinutes) : base(actionName, targetRepeatCount)
        {
            _repeatTimePeriod = repeatTimePeriod;
            _completionTime = minCompletionTimeMinutes;
        }
    }
}
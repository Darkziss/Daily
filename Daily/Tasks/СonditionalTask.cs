
namespace Daily.Tasks
{
    public class СonditionalTask : TaskBase
    {
        private TaskRepeatTimePeriod _repeatTimePeriod;

        private int _completionTime = 0;

        private string _note = string.Empty;

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

        public string Note
        {
            get => _note;
            set
            {
                if (value.Equals(_note)) return;

                _note = value;
                OnPropertyChanged(nameof(Note));
            }
        }

        public СonditionalTask(string actionName, int repeatCount, int targetRepeatCount, 
            TaskRepeatTimePeriod repeatTimePeriod, int completionTime, string note) 
            : base(actionName, repeatCount, targetRepeatCount)
        {
            RepeatTimePeriod = repeatTimePeriod;
            CompletionTime = completionTime;
            Note = note;
        }
    }
}
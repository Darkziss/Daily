using Daily.Events;

namespace Daily.Tasks
{
    public abstract class TaskBase : NotifyPropertyChanged
    {
        private string _actionName = string.Empty;

        private int _repeatCount = 0;
        private int _targetRepeatCount;

        private string _note = string.Empty;

        public string ActionName
        {
            get => _actionName;
            set
            {
                if (value.Equals(_actionName)) return;

                _actionName = value;
                OnPropertyChanged(nameof(ActionName));
            }
        }

        public int RepeatCount
        {
            get => _repeatCount;
            set
            {
                if (value == _repeatCount) return;

                _repeatCount = value;
                OnPropertyChanged(nameof(RepeatCount));
            }
        }
        public int TargetRepeatCount
        {
            get => _targetRepeatCount;
            set
            {
                if (value == _targetRepeatCount) return;

                _targetRepeatCount = value;
                OnPropertyChanged(nameof(TargetRepeatCount));
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

        public bool IsCompleted => RepeatCount == TargetRepeatCount;
        
        public TaskBase(string actionName, int repeatCount, int targetRepeatCount, string note)
        {
            ActionName = actionName;
            RepeatCount = repeatCount;
            TargetRepeatCount = targetRepeatCount;
            Note = note;
        }

        public void Perform()
        {
            if (IsCompleted) return;

            RepeatCount++;

            if (IsCompleted) OnPropertyChanged(nameof(IsCompleted));
        }

        public void Reset()
        {
            if (RepeatCount == 0) return;

            bool wasCompleted = IsCompleted;

            RepeatCount = 0;

            if (wasCompleted) OnPropertyChanged(nameof(IsCompleted));
        }
    }
}
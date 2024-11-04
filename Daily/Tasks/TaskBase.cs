using System.ComponentModel;

namespace Daily.Tasks
{
    public class TaskBase : INotifyPropertyChanged
    {
        private string _actionName = string.Empty;

        private int _repeatCount = 0;
        private int _targetRepeatCount;

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

        public bool IsCompleted => RepeatCount == TargetRepeatCount;
        
        public event PropertyChangedEventHandler? PropertyChanged;

        public TaskBase(string actionName, int targetRepeatCount)
        {
            ActionName = actionName;
            TargetRepeatCount = targetRepeatCount;
        }

        public void Perform()
        {
            if (IsCompleted) return;

            RepeatCount++;

            if (IsCompleted) OnPropertyChanged(nameof(IsCompleted));
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
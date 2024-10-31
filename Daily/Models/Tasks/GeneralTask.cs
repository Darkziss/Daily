using System.ComponentModel;

namespace Daily.Tasks
{
    public class GeneralTask : INotifyPropertyChanged
    {
        private string _actionName = string.Empty;
        private TaskPriority _priority;

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

        public GeneralTask(string actionName, TaskPriority priority, int targetRepeatCount)
        {
            ActionName = actionName;
            Priority = priority;
            TargetRepeatCount = targetRepeatCount;
        }

        public void Perform()
        {
            if (IsCompleted) return;

            RepeatCount++;

            if (IsCompleted) OnPropertyChanged(nameof(IsCompleted));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
    }
}
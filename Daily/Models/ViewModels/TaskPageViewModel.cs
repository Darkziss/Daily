using CommunityToolkit.Mvvm.ComponentModel;

namespace Daily.ViewModels
{
    public partial class TaskPageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isGoalLabelVisible = true;
        [ObservableProperty] private bool _isGoalEntryVisible = false;

        public Command ShowGoalLabelCommand { get; private set; }

        public Command ShowGoalEntryCommand { get; private set; }

        public TaskPageViewModel()
        {
            ShowGoalLabelCommand = new Command(() =>
            {
                if (IsGoalLabelVisible) return;

                IsGoalEntryVisible = false;
                IsGoalLabelVisible = true;
            });
            
            ShowGoalEntryCommand = new Command(() =>
            {
                if (IsGoalEntryVisible) return;
                
                IsGoalLabelVisible = false;
                IsGoalEntryVisible = true;
            });
        }
    }
}
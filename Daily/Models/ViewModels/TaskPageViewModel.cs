using CommunityToolkit.Mvvm.ComponentModel;

namespace Daily.ViewModels
{
    public partial class TaskPageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isGoalLabelVisible = true;
        [ObservableProperty] private bool _isGoalEntryVisible = false;

        [ObservableProperty] private string _goalLabelText;

        public Command ChangeGoalCommand { get; private set; }
        public Command SaveGoalCommand { get; private set; }

        private const string goalLabelDefaultText = "Зажмите, чтобы изменить текст";

        public TaskPageViewModel()
        {
            _goalLabelText = goalLabelDefaultText;
            
            ChangeGoalCommand = new Command(
            execute: () =>
            {
                IsGoalLabelVisible = false;
                IsGoalEntryVisible = true;
            }, 
            canExecute: () =>
            {
                return !IsGoalEntryVisible;
            });

            SaveGoalCommand = new Command(
            execute: (args) =>
            {
                IsGoalEntryVisible = false;

                string text = (string)args;
                if (text != GoalLabelText) GoalLabelText = text;

                IsGoalLabelVisible = true;
            },
            canExecute: (_) =>
            {
                return !IsGoalLabelVisible;
            });
        }
    }
}
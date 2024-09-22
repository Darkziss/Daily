using CommunityToolkit.Mvvm.ComponentModel;

namespace Daily.ViewModels
{
    public partial class TaskPageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isGoalLabelVisible = true;
        [ObservableProperty] private bool _isGoalEntryVisible = false;

        [ObservableProperty] private string _goalLabelText;

        private readonly GoalStorage _goalStorage;

        public Command ChangeGoalCommand { get; private set; }
        public Command SaveGoalCommand { get; private set; }

        private const string goalLabelDefaultText = "Зажмите, чтобы добавить цель";

        public TaskPageViewModel(GoalStorage goalStorage)
        {
            _goalStorage = goalStorage;

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
                bool isNullOrWhiteSpace = string.IsNullOrWhiteSpace(text);

                if (isNullOrWhiteSpace) GoalLabelText = goalLabelDefaultText;
                else if (text != GoalLabelText) GoalLabelText = text;

                IsGoalLabelVisible = true;
            },
            canExecute: (_) =>
            {
                return !IsGoalLabelVisible;
            });
        }

        public void PreparePage()
        {
            _isGoalLabelVisible = true;
            _isGoalEntryVisible = false;
        }
    }
}
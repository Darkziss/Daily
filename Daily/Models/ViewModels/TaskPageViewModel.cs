using CommunityToolkit.Mvvm.ComponentModel;

namespace Daily.ViewModels
{
    public partial class TaskPageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isGoalLabelVisible = true;
        [ObservableProperty] private bool _isGoalEntryVisible = false;

        [ObservableProperty] private string _goalLabelText;

        private readonly GoalStorage _goalStorage;

        public Command EditGoalCommand { get; private set; }
        public Command SaveGoalCommand { get; private set; }

        private const string goalLabelDefaultText = "Зажмите, чтобы добавить цель";

        public TaskPageViewModel(GoalStorage goalStorage)
        {
            _goalStorage = goalStorage;

            _goalLabelText = GetGoalOrDefault();
            
            EditGoalCommand = new Command(
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
            execute: async (args) =>
            {
                IsGoalEntryVisible = false;

                string text = (string)args;

                bool isSameGoal = _goalStorage.IsSameGoal(text);

                if (!isSameGoal)
                {
                    await _goalStorage.SetGoalAsync(text);

                    GoalLabelText = GetGoalOrDefault();
                }

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

        private string GetGoalOrDefault()
        {
            bool isNullOrWhiteSpace = string.IsNullOrWhiteSpace(_goalStorage.Goal);

            return isNullOrWhiteSpace ? goalLabelDefaultText : _goalStorage.Goal;
        }
    }
}
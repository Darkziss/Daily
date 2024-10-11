using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Tasks;

namespace Daily.ViewModels
{
    public partial class TaskPageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isGoalLabelVisible = true;
        [ObservableProperty] private bool _isGoalEntryVisible = false;

        [ObservableProperty] private string _goalLabelText;
        [ObservableProperty] private string _goalEntryText;

        private readonly GoalStorage _goalStorage;
        private readonly TaskStorage _taskStorage;

        public IReadOnlyList<GeneralTask> GeneralTasks => _taskStorage.GeneralTasks;

        public Command EditGoalCommand { get; }
        public Command SaveGoalCommand { get; }

        private const string goalLabelDefaultText = "Зажмите, чтобы добавить цель";

        public TaskPageViewModel(GoalStorage goalStorage, TaskStorage taskStorage)
        {
            _goalStorage = goalStorage;
            _taskStorage = taskStorage;

            _goalLabelText = GetGoalOrDefaultText();
            _goalEntryText = goalStorage.Goal;

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
            execute: async () =>
            {
                IsGoalEntryVisible = false;

                string newGoal = GoalEntryText;
                bool isSameGoal = _goalStorage.IsSameGoal(newGoal);

                if (!isSameGoal)
                {
                    await _goalStorage.SetGoalAsync(newGoal);

                    GoalLabelText = GetGoalOrDefaultText();
                }

                IsGoalLabelVisible = true;
            },
            canExecute: () =>
            {
                return !IsGoalLabelVisible;
            });
        }

        public void PreparePage()
        {
            IsGoalLabelVisible = true;
            IsGoalEntryVisible = false;
        }

        private string GetGoalOrDefaultText()
        {
            bool isNullOrWhiteSpace = string.IsNullOrWhiteSpace(_goalStorage.Goal);

            return isNullOrWhiteSpace ? goalLabelDefaultText : _goalStorage.Goal;
        }
    }
}
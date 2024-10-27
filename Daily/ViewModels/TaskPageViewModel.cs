using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Tasks;

namespace Daily.ViewModels
{
    public partial class TaskPageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isEditingGoal = false;

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
                IsEditingGoal = true;
            }, 
            canExecute: () =>
            {
                return !IsEditingGoal;
            });

            SaveGoalCommand = new Command(
            execute: async () =>
            {
                string newGoal = GoalEntryText;
                bool isSameGoal = _goalStorage.IsSameGoal(newGoal);

                if (!isSameGoal)
                {
                    await _goalStorage.SetGoalAsync(newGoal);

                    GoalLabelText = GetGoalOrDefaultText();
                }

                IsEditingGoal = false;
            },
            canExecute: () =>
            {
                return IsEditingGoal;
            });
        }

        public void PreparePage()
        {
            IsEditingGoal = false;
        }

        private string GetGoalOrDefaultText()
        {
            bool isNullOrWhiteSpace = string.IsNullOrWhiteSpace(_goalStorage.Goal);

            return isNullOrWhiteSpace ? goalLabelDefaultText : _goalStorage.Goal;
        }
    }
}
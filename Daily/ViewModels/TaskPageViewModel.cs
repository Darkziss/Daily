using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Tasks;
using AsyncTimer = System.Timers.Timer;

namespace Daily.ViewModels
{
    public partial class TaskPageViewModel : ObservableObject, IPrepareView
    {
        [ObservableProperty] private bool _isEditingGoal = false;

        [ObservableProperty] private string _goalLabelText;
        [ObservableProperty] private string _goalEntryText;

        [ObservableProperty] private object? _selectedTask = null;

        [ObservableProperty] private bool _isGeneralTasksLoaded = false;

        private readonly GoalStorage _goalStorage;
        private readonly TaskStorage _taskStorage;

        public IReadOnlyList<GeneralTask> GeneralTasks => _taskStorage.GeneralTasks;
        public IReadOnlyList<СonditionalTask> СonditionalTasks => _taskStorage.СonditionalTasks;

        public Command EditGoalCommand { get; }
        public Command SaveGoalCommand { get; }

        public Command<GeneralTask> TaskPerformedCommand { get; }

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

            TaskPerformedCommand = new Command<GeneralTask>(
            execute: async (task) =>
            {
                await _taskStorage.PerformGeneralTaskAsync(task);

                SelectedTask = null;
            },
            canExecute: (task) =>
            {
                return task == null ? false : !task.IsCompleted;
            });

            PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(SelectedTask))
                {
                    TaskPerformedCommand.ChangeCanExecute();
                }
            };
        }

        public void PrepareView()
        {
            IsEditingGoal = false;
            IsGeneralTasksLoaded = false;

            TaskPerformedCommand.ChangeCanExecute();

            const double delay = 800d;
            AsyncTimer timer = new AsyncTimer(delay);

            timer.Elapsed += (_, _) =>
            {
                timer.Stop();
                timer.Dispose();

                IsGeneralTasksLoaded = true;
            };

            timer.Start();
        }

        private string GetGoalOrDefaultText()
        {
            bool isNullOrWhiteSpace = string.IsNullOrWhiteSpace(_goalStorage.Goal);

            return isNullOrWhiteSpace ? goalLabelDefaultText : _goalStorage.Goal;
        }
    }
}
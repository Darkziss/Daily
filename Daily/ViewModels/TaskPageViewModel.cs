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
        [ObservableProperty] private object? _selectedСonditionalTask = null;

        [ObservableProperty] private bool _isTasksLoaded = false;

        private readonly GoalStorage _goalStorage;
        private readonly TaskStorage _taskStorage;

        public IReadOnlyList<GeneralTask> GeneralTasks => _taskStorage.GeneralTasks;
        public IReadOnlyList<СonditionalTask> СonditionalTasks => _taskStorage.СonditionalTasks;

        public Command EditGoalCommand { get; }
        public Command SaveGoalCommand { get; }

        public Command<GeneralTask> TaskPerformedCommand { get; }
        public Command<СonditionalTask> ConditionalTaskPerformedCommand { get; }

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

            bool CanPerformTask(TaskBase task) => task == null ? false : !task.IsCompleted;

            TaskPerformedCommand = new Command<GeneralTask>(
            execute: async (task) =>
            {
                await _taskStorage.PerformGeneralTaskAsync(task);

                SelectedTask = null;
            },
            canExecute: CanPerformTask);

            ConditionalTaskPerformedCommand = new Command<СonditionalTask>(
            execute: async (task) =>
            {
                await _taskStorage.PerformСonditionalTaskAsync(task);

                SelectedСonditionalTask = null;
            },
            canExecute: CanPerformTask);

            PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(SelectedTask))
                {
                    TaskPerformedCommand.ChangeCanExecute();
                }
                else if (args.PropertyName == nameof(SelectedСonditionalTask))
                {
                    ConditionalTaskPerformedCommand.ChangeCanExecute();
                }
            };
        }

        public void PrepareView()
        {
            IsEditingGoal = false;
            IsTasksLoaded = false;

            TaskPerformedCommand.ChangeCanExecute();
            ConditionalTaskPerformedCommand.ChangeCanExecute();

            const double delay = 800d;
            AsyncTimer timer = new AsyncTimer(delay);

            timer.Elapsed += (_, _) =>
            {
                timer.Stop();
                timer.Dispose();

                IsTasksLoaded = true;
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
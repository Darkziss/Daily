using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Tasks;
using System.Diagnostics;
using AsyncTimer = System.Timers.Timer;

namespace Daily.ViewModels
{
    public partial class TaskPageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isEditingGoal = false;

        [ObservableProperty] private string _goalLabelText;
        [ObservableProperty] private string _goalEntryText;

        [ObservableProperty] private bool _isGeneralTasksLoaded = false;

        private readonly GoalStorage _goalStorage;
        private readonly TaskStorage _taskStorage;

        public IReadOnlyList<GeneralTask> GeneralTasks => _taskStorage.GeneralTasks;

        public Command EditGoalCommand { get; }
        public Command SaveGoalCommand { get; }

        public Command TaskPerformedCommand { get; }

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

            TaskPerformedCommand = new Command(
            execute: (obj) =>
            {
                GeneralTask task = (GeneralTask)obj;

                for (int i = 0; i < GeneralTasks.Count; i++)
                {
                    bool isSameTask = string.Equals(task.ActionName, GeneralTasks[i].ActionName);

                    if (isSameTask)
                    {
                        _taskStorage.MarkGeneralTaskAsCompleted(i);
                        break;
                    }
                }

                Debug.WriteLine($"Marking task: {task.ActionName}");
                Debug.WriteLine($"Progress: {task.RepeatCount}/{task.TargetRepeatCount}");
            },
            canExecute: (obj) =>
            {
                GeneralTask task = (GeneralTask)obj;

                return !task.IsCompleted;
            });
        }

        public void PreparePage()
        {
            IsEditingGoal = false;
            IsGeneralTasksLoaded = false;

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
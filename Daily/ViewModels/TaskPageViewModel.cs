using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Tasks;
using Daily.Pages;
using AsyncTimer = System.Timers.Timer;
using System.Diagnostics;

namespace Daily.ViewModels
{
    public partial class TaskPageViewModel : ObservableObject, IPrepareView
    {
        [ObservableProperty] private bool _isEditingGoal = false;

        [ObservableProperty] private string _goalLabelText;
        [ObservableProperty] private string _goalEntryText;

        [ObservableProperty] private object? _selectedGeneralTask = null;
        [ObservableProperty] private object? _selectedСonditionalTask = null;

        [ObservableProperty] private bool _isTasksLoaded = false;

        [ObservableProperty] private bool _canAddTask = true;
        [ObservableProperty] private bool _canEditTask = false;
        [ObservableProperty] private bool _canDeleteTask = false;

        private readonly GoalStorage _goalStorage;
        private readonly TaskStorage _taskStorage;

        public IReadOnlyList<GeneralTask> GeneralTasks => _taskStorage.GeneralTasks;
        public IReadOnlyList<СonditionalTask> СonditionalTasks => _taskStorage.СonditionalTasks;

        public Command EditGoalCommand { get; }
        public Command SaveGoalCommand { get; }

        public Command<GeneralTask> GeneralTaskInteractCommand { get; }
        public Command<СonditionalTask> СonditionalTaskInteractCommand { get; }

        public Command AddTaskCommand { get; }
        public Command SwitchCanEditTaskCommand { get; }
        public Command SwitchCanDeleteTaskCommand { get; }

        private const string goalLabelDefaultText = "Зажмите, чтобы добавить цель";

        private const string generalTaskParameterName = "GeneralTask";

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

            GeneralTaskInteractCommand = new Command<GeneralTask>(
            execute: async (task) =>
            {
                if (SelectedGeneralTask == null) return;
                
                if (CanEditTask)
                {
                    CanAddTask = false;
                    
                    var parameters = new ShellNavigationQueryParameters()
                    {
                        [nameof(GeneralTask)] = task
                    };

                    await PageRouter.RouteToPageWithParameters(nameof(TaskEditPage), parameters);
                }
                else
                {
                    await PerformGeneralTaskAsync(task);
                }

                SelectedGeneralTask = null;
            });

            СonditionalTaskInteractCommand = new Command<СonditionalTask>(
            execute: async (task) =>
            {
                if (SelectedСonditionalTask == null) return;
                
                if (CanEditTask)
                {
                    CanAddTask = false;

                    var parameters = new ShellNavigationQueryParameters()
                    {
                        [nameof(СonditionalTask)] = task
                    };

                    await PageRouter.RouteToPageWithParameters(nameof(TaskEditPage), parameters);
                }
                else
                {
                    await PerformСonditionalTaskAsync(task);
                }

                SelectedСonditionalTask = null;
            });

            AddTaskCommand = new Command(
            execute: async () =>
            {
                CanAddTask = false;
                
                await PageRouter.RouteToPage(nameof(TaskEditPage));
            });

            SwitchCanEditTaskCommand = new Command(() => CanEditTask = !CanEditTask);

            SwitchCanDeleteTaskCommand = new Command(() => CanDeleteTask = !CanDeleteTask);
        }

        public void ResetView()
        {
            IsEditingGoal = false;
            IsTasksLoaded = false;

            CanAddTask = true;
            CanEditTask = false;
            CanDeleteTask = false;

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

        private async Task PerformGeneralTaskAsync(GeneralTask task)
        {
            if (!CanPerformTask(task)) return;

            await _taskStorage.PerformGeneralTaskAsync(task);
        }

        private async Task PerformСonditionalTaskAsync(СonditionalTask task)
        {
            if (!CanPerformTask(task)) return;

            await _taskStorage.PerformСonditionalTaskAsync(task);
        }

        private bool CanPerformTask(TaskBase task) => task == null ? false : !task.IsCompleted;
    }
}
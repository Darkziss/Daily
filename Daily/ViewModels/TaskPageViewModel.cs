using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Tasks;
using Daily.Navigation;
using Daily.Toasts;
using Daily.Popups;
using AsyncTimer = System.Timers.Timer;

namespace Daily.ViewModels
{
    public partial class TaskPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private bool _isGoalEmpty;
        
        [ObservableProperty] private bool _isEditingGoal = false;

        [ObservableProperty] private string _goalLabelText;
        [ObservableProperty] private string _goalEntryText;

        [ObservableProperty] private object? _selectedGeneralTask = null;
        [ObservableProperty] private object? _selectedСonditionalTask = null;

        [ObservableProperty] private bool _isTasksLoaded = false;

        [ObservableProperty] private bool _canInteractWithTask = true;

        [ObservableProperty] private bool _canEditTask = false;
        [ObservableProperty] private bool _canDeleteTask = false;
        [ObservableProperty] private bool _canResetTask = false;

        private readonly GoalStorage _goalStorage;
        private readonly GeneralTaskStorage _generalTaskStorage;
        private readonly ConditionalTaskStorage _conditionalTaskStorage;

        public DateTime DeadlineMinimumDate => DateTime.Now.AddDays(1d);

        public ObservableCollection<GeneralTask> GeneralTasks => _generalTaskStorage.Tasks;
        public ObservableCollection<СonditionalTask> СonditionalTasks => _conditionalTaskStorage.Tasks;

        public int GeneralTaskMaxCount => _generalTaskStorage.MaxTaskCount;
        public int ConditionalTaskMaxCount => _conditionalTaskStorage.MaxTaskCount;

        public Command EditGoalCommand { get; }
        public Command SaveGoalCommand { get; }

        public Command<GeneralTask> GeneralTaskInteractCommand { get; }
        public Command<СonditionalTask> СonditionalTaskInteractCommand { get; }

        public Command AddTaskCommand { get; }

        public Command SwitchCanEditTaskCommand { get; }
        public Command SwitchCanDeleteTaskCommand { get; }
        public Command SwitchCanResetTaskCommand { get; }

        private bool ShouldLoadTask => GeneralTasks.Count > 0 || СonditionalTasks.Count > 0;

        private const string goalLabelDefaultText = "Зажмите, чтобы добавить цель";

        public TaskPageViewModel(GoalStorage goalStorage, GeneralTaskStorage generalTaskStorage, 
            ConditionalTaskStorage conditionalTaskStorage)
        {
            _goalStorage = goalStorage;

            _generalTaskStorage = generalTaskStorage;
            _conditionalTaskStorage = conditionalTaskStorage;

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
                if (SelectedGeneralTask == null || !CanInteractWithTask) return;

                if (CanEditTask)
                {
                    CanInteractWithTask = false;

                    var parameters = new ShellNavigationQueryParameters()
                    {
                        [nameof(GeneralTask)] = task
                    };

                    await PageNavigator.GoToTaskEditPageAsync(parameters);
                }
                else if (CanDeleteTask)
                {
                    CanInteractWithTask = false;

                    bool shouldDelete = await PopupHandler.ShowTaskDeletePopupAsync(task.ActionName);

                    if (shouldDelete)
                    {
                        await _generalTaskStorage.DeleteTaskAsync(task);
                        await TaskToastHandler.ShowTaskDeletedToastAsync();
                    }

                    CanInteractWithTask = true;
                }
                else if (CanResetTask) await ResetGeneralTaskAsync(task);
                else await PerformGeneralTaskAsync(task);

                SelectedGeneralTask = null;
            });

            СonditionalTaskInteractCommand = new Command<СonditionalTask>(
            execute: async (task) =>
            {
                if (SelectedСonditionalTask == null || !CanInteractWithTask) return;
                
                if (CanEditTask)
                {
                    CanInteractWithTask = false;

                    var parameters = new ShellNavigationQueryParameters()
                    {
                        [nameof(СonditionalTask)] = task
                    };

                    await PageNavigator.GoToTaskEditPageAsync(parameters);
                }
                else if (CanDeleteTask)
                {
                    CanInteractWithTask = false;

                    bool shouldDelete = await PopupHandler.ShowTaskDeletePopupAsync(task.ActionName);

                    if (shouldDelete)
                    {
                        await _conditionalTaskStorage.DeleteTaskAsync(task);
                        await TaskToastHandler.ShowTaskDeletedToastAsync();
                    }
                    
                    CanInteractWithTask = true;
                }
                else if (CanResetTask) await ResetConditionalTaskAsync(task);
                else await PerformСonditionalTaskAsync(task);

                SelectedСonditionalTask = null;
            });

            AddTaskCommand = new Command(
            execute: async () =>
            {
                CanInteractWithTask = false;

                await PageNavigator.GoToTaskEditPageAsync();
            });

            SwitchCanEditTaskCommand = new Command(
            execute: () =>
            {
                CanEditTask = !CanEditTask;

                CanDeleteTask = false;
                CanResetTask = false;
            });

            SwitchCanDeleteTaskCommand = new Command(
            execute: () =>
            {
                CanDeleteTask = !CanDeleteTask;

                CanEditTask = false;
                CanResetTask = false;
            });

            SwitchCanResetTaskCommand = new Command(
            execute: () =>
            {
                CanResetTask = !CanResetTask;

                CanEditTask = false;
                CanDeleteTask = false;
            });

            PropertyChanged += (_, args) =>
            {
                string? name = args.PropertyName;
                
                if (name == nameof(GoalEntryText))
                {
                    IsGoalEmpty = GoalEntryText.Length == 0;
                }
            };
        }

        public void ResetView()
        {
            IsEditingGoal = false;

            CanEditTask = false;
            CanDeleteTask = false;
            CanResetTask = false;

            if (ShouldLoadTask) ShowDummy();
            else
            {
                IsTasksLoaded = true;
                CanInteractWithTask = true;
            }
        }

        private string GetGoalOrDefaultText()
        {
            bool isNullOrWhiteSpace = string.IsNullOrWhiteSpace(_goalStorage.Goal);

            return isNullOrWhiteSpace ? goalLabelDefaultText : _goalStorage.Goal;
        }

        private async Task PerformGeneralTaskAsync(GeneralTask task)
        {
            if (!CanPerformTask(task)) return;

            await _generalTaskStorage.PerformTaskAsync(task);
        }

        private async Task PerformСonditionalTaskAsync(СonditionalTask task)
        {
            if (!CanPerformTask(task)) return;

            await _conditionalTaskStorage.PerformTaskAsync(task);
        }

        private async Task ResetGeneralTaskAsync(GeneralTask task)
        {
            if (task == null || task.RepeatCount == 0) return;

            await _generalTaskStorage.ResetTaskAsync(task);
        }

        private async Task ResetConditionalTaskAsync(СonditionalTask task)
        {
            if (task == null || task.RepeatCount == 0) return;

            await _conditionalTaskStorage.ResetTaskAsync(task);
        }

        private bool CanPerformTask(TaskBase task) => task == null ? false : !task.IsCompleted;

        private void ShowDummy()
        {
            IsTasksLoaded = false;
            CanInteractWithTask = false;

            const double delay = 800d;
            AsyncTimer timer = new AsyncTimer(delay);

            timer.Elapsed += (_, _) =>
            {
                timer.Stop();

                IsTasksLoaded = true;
                CanInteractWithTask = true;
            };

            timer.Start();
        }
    }
}
using System.Collections.ObjectModel;
using Daily.Tasks;
using Daily.Navigation;
using Daily.Toasts;
using Daily.Popups;
using Daily.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using AsyncTimer = System.Timers.Timer;
using System.Diagnostics;

namespace Daily.ViewModels
{
    public partial class TaskPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private bool _isGoalEmpty;
        [ObservableProperty] private bool _hasDeadline;
        
        [ObservableProperty] private string? _goalLabelText;
        [ObservableProperty] private DateOnly? _deadline;

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

        public ObservableCollection<GeneralTask> GeneralTasks => _generalTaskStorage.Tasks;
        public ObservableCollection<СonditionalTask> СonditionalTasks => _conditionalTaskStorage.Tasks;

        public int GeneralTaskMaxCount => _generalTaskStorage.MaxTaskCount;
        public int ConditionalTaskMaxCount => _conditionalTaskStorage.MaxTaskCount;

        public Command EditGoalCommand { get; }
        public Command CompleteGoalCommand { get; }

        public Command<GeneralTask> GeneralTaskInteractCommand { get; }
        public Command<СonditionalTask> СonditionalTaskInteractCommand { get; }

        public Command AddTaskCommand { get; }

        public Command SwitchCanEditTaskCommand { get; }
        public Command SwitchCanDeleteTaskCommand { get; }
        public Command SwitchCanResetTaskCommand { get; }

        private bool ShouldLoadTask => GeneralTasks.Count > 0 || СonditionalTasks.Count > 0;

        private const string goalLabelDefaultText = "Здесь пока пусто..";

        public TaskPageViewModel(GoalStorage goalStorage, GeneralTaskStorage generalTaskStorage, 
            ConditionalTaskStorage conditionalTaskStorage)
        {
            _goalStorage = goalStorage;

            _generalTaskStorage = generalTaskStorage;
            _conditionalTaskStorage = conditionalTaskStorage;

            _goalLabelText = _goalStorage.Goal;
            _deadline = _goalStorage.Deadline;

            EditGoalCommand = new Command(async () =>
            {
                await PageNavigator.GoToGoalEditPageAsync();
            });

            CompleteGoalCommand = new Command(() =>
            {
                Debug.WriteLine(nameof(CompleteGoalCommand));
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

            WeakReferenceMessenger.Default.Register<GoalChangedMessage>(this, (_, _) =>
            {
                GoalLabelText = _goalStorage.Goal;
                Deadline = _goalStorage.Deadline;

                UpdateGoalAndDeadlineStatus();
            });
        }

        public void ResetView()
        {
            UpdateGoalAndDeadlineStatus();
            
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

        private void UpdateGoalAndDeadlineStatus()
        {
            IsGoalEmpty = string.IsNullOrEmpty(_goalStorage.Goal);
            HasDeadline = _goalStorage.Deadline.HasValue;
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
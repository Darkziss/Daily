using System.ComponentModel;
using System.Collections.ObjectModel;
using Daily.Tasks;
using Daily.Navigation;
using Daily.Toasts;
using Daily.Popups;
using Daily.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Sharpnado.TaskLoaderView;
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

        [ObservableProperty] private GoalStatus? _goalStatus;

        [ObservableProperty] private ObservableCollection<ConditionalTask> _conditionalTasks;

        [ObservableProperty] private object? _selectedGeneralTask = null;
        [ObservableProperty] private object? _selectedСonditionalTask = null;

        [ObservableProperty] private bool _isTasksVisible = false;

        [ObservableProperty] private bool _canInteractWithTask = true;

        [ObservableProperty] private bool _canEditTask = false;
        [ObservableProperty] private bool _canDeleteTask = false;
        [ObservableProperty] private bool _canResetTask = false;

        private readonly GoalStorage _goalStorage;
        private readonly GeneralTaskStorage _generalTaskStorage;
        private readonly ConditionalTaskStorage _conditionalTaskStorage;

        public TaskLoaderNotifier<Goal> GoalLoader { get; }

        public TaskLoaderNotifier<ObservableCollection<GeneralTask>> GeneralTasksLoader { get; }
        public TaskLoaderNotifier<ObservableCollection<ConditionalTask>> ConditionalTasksLoader { get; }

        public int GeneralTaskMaxCount => _generalTaskStorage.MaxTaskCount;
        public int ConditionalTaskMaxCount => _conditionalTaskStorage.MaxTaskCount;

        public Command EditGoalCommand { get; }
        public Command InvertGoalStatusCommand { get; }

        public Command<GeneralTask> GeneralTaskInteractCommand { get; }
        public Command<ConditionalTask> СonditionalTaskInteractCommand { get; }

        public Command AddTaskCommand { get; }

        public Command SwitchCanEditTaskCommand { get; }
        public Command SwitchCanDeleteTaskCommand { get; }
        public Command SwitchCanResetTaskCommand { get; }

        public TaskPageViewModel(GoalStorage goalStorage, GeneralTaskStorage generalTaskStorage, 
            ConditionalTaskStorage conditionalTaskStorage)
        {
            _goalStorage = goalStorage;

            _generalTaskStorage = generalTaskStorage;
            _conditionalTaskStorage = conditionalTaskStorage;

            GoalLoader = new(true);

            GeneralTasksLoader = new(true);
            ConditionalTasksLoader = new(true);

            EditGoalCommand = new Command(async () =>
            {
                if (!PageNavigator.IsRouting)
                    await PageNavigator.GoToGoalEditPageAsync();
            });

            InvertGoalStatusCommand = new Command(async () =>
            {
                if (_goalStorage.IsNone)
                    return;
                
                if (_goalStorage.IsCompleted) await _goalStorage.ResetGoalStatusAsync();
                else await _goalStorage.CompleteGoalAsync();

                GoalStatus = _goalStorage.Status;
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

            СonditionalTaskInteractCommand = new Command<ConditionalTask>(
            execute: async (task) =>
            {
                if (SelectedСonditionalTask == null || !CanInteractWithTask) return;
                
                if (CanEditTask)
                {
                    CanInteractWithTask = false;

                    var parameters = new ShellNavigationQueryParameters()
                    {
                        [nameof(ConditionalTask)] = task
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
                GoalStatus = _goalStorage.Status;

                UpdateGoalAndDeadlineStatus();
            });
        }

        public void ResetView()
        {
            LoadGoalIfNotLoaded();
            LoadTasksIfNotLoaded();

            if (GoalLoader.IsSuccessfullyCompleted)
            {
                RefreshOverdueStatusIfGoalNotCompleted();

                UpdateGoalAndDeadlineStatus();
            }

            ShowDummy();

            CanEditTask = false;
            CanDeleteTask = false;
            CanResetTask = false;
        }

        private void LoadGoalIfNotLoaded()
        {
            if (GoalLoader.IsNotStarted)
            {
                GoalLoader.PropertyChanged += GoalLoader_PropertyChanged;
                GoalLoader.Load(_ => _goalStorage.LoadGoalAsync());
            }
        }

        private void GoalLoader_PropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            const string ResultPropertyName = "Result";

            if (GoalLoader.IsSuccessfullyCompleted && args.PropertyName == ResultPropertyName)
            {
                GoalLabelText = _goalStorage.Goal;
                Deadline = _goalStorage.Deadline;

                GoalStatus = _goalStorage.Status;

                RefreshOverdueStatusIfGoalNotCompleted();

                UpdateGoalAndDeadlineStatus();

                GoalLoader.PropertyChanged -= GoalLoader_PropertyChanged;
            }
        }

        private void LoadTasksIfNotLoaded()
        {
            if (GeneralTasksLoader.IsNotStarted)
                GeneralTasksLoader.Load(_ => _generalTaskStorage.LoadTasks());

            if (ConditionalTasksLoader.IsNotStarted)
                ConditionalTasksLoader.Load(_ => _conditionalTaskStorage.LoadTasks());
        }

        private void RefreshOverdueStatusIfGoalNotCompleted()
        {
            if (!_goalStorage.IsCompleted)
            {
                _goalStorage.RefreshOverdueStatus();

                GoalStatus = _goalStorage.Status;
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

        private async Task PerformСonditionalTaskAsync(ConditionalTask task)
        {
            if (!CanPerformTask(task)) return;

            await _conditionalTaskStorage.PerformTaskAsync(task);
        }

        private async Task ResetGeneralTaskAsync(GeneralTask task)
        {
            if (task == null || task.RepeatCount == 0) return;

            await _generalTaskStorage.ResetTaskAsync(task);
        }

        private async Task ResetConditionalTaskAsync(ConditionalTask task)
        {
            if (task == null || task.RepeatCount == 0) return;

            await _conditionalTaskStorage.ResetTaskAsync(task);
        }

        private bool CanPerformTask(TaskBase task) => task == null ? false : !task.IsCompleted;

        private void ShowDummy()
        {
            IsTasksVisible = false;
            CanInteractWithTask = false;

            const double delay = 800d;
            AsyncTimer timer = new AsyncTimer(delay);

            timer.Elapsed += (_, _) =>
            {
                timer.Stop();
                timer.Dispose();

                IsTasksVisible = true;
                CanInteractWithTask = true;
            };

            timer.Start();
        }
    }
}
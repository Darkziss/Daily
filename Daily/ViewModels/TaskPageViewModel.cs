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

        [ObservableProperty] private bool _isTasksVisible = false;

        [ObservableProperty] private bool _canInteractWithTask = true;

        private readonly GoalStorage _goalStorage;
        private readonly OneTimeTaskStorage _oneTimeTaskStorage;
        private readonly ConditionalTaskStorage _conditionalTaskStorage;

        public TaskLoaderNotifier<Goal> GoalLoader { get; }

        public TaskLoaderNotifier<ObservableCollection<OneTimeTask>> OneTimeTasksLoader { get; }
        public TaskLoaderNotifier<ObservableCollection<ConditionalTask>> ConditionalTasksLoader { get; }

        public int OneTimeTaskMaxCount => _oneTimeTaskStorage.MaxTaskCount;
        public int ConditionalTaskMaxCount => _conditionalTaskStorage.MaxTaskCount;

        public Command EditGoalCommand { get; }
        public Command InvertGoalStatusCommand { get; }

        public Command<ConditionalTask> PerformConditionalTaskCommand { get; }
        public Command<ConditionalTask> EditConditionalTaskCommand { get; }
        public Command<ConditionalTask> ResetConditionalTaskCommand { get; }
        public Command<ConditionalTask> DeleteConditionalTaskCommand { get; }

        public Command<OneTimeTask> PerformOneTimeTaskCommand { get; }
        public Command<OneTimeTask> EditOneTimeTaskCommand { get; }
        public Command<OneTimeTask> ResetOneTimeTaskCommand { get; }
        public Command<OneTimeTask> DeleteOneTimeTaskCommand { get; }

        public Command AddTaskCommand { get; }

        public TaskPageViewModel(GoalStorage goalStorage, OneTimeTaskStorage oneTimeTaskStorage, 
            ConditionalTaskStorage conditionalTaskStorage)
        {
            _goalStorage = goalStorage;

            _oneTimeTaskStorage = oneTimeTaskStorage;
            _conditionalTaskStorage = conditionalTaskStorage;

            GoalLoader = new(true);

            OneTimeTasksLoader = new(true);
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

            PerformConditionalTaskCommand = new(async (task) =>
            {
                if (!CanInteractWithTask)
                    return;

                await PerformСonditionalTaskAsync(task);
            });

            EditConditionalTaskCommand = new(async (task) =>
            {
                if (!CanInteractWithTask)
                    return;

                var parameters = new ShellNavigationQueryParameters()
                {
                    [nameof(ConditionalTask)] = task
                };

                await PageNavigator.GoToTaskEditPageAsync(parameters);
            });

            ResetConditionalTaskCommand = new(async (task) =>
            {
                if (!CanInteractWithTask)
                    return;

                await ResetConditionalTaskAsync(task);
            });

            DeleteConditionalTaskCommand = new(async (task) =>
            {
                if (!CanInteractWithTask)
                    return;

                bool shouldDelete = await PopupHandler.ShowTaskDeletePopupAsync(task.ActionName);

                if (shouldDelete)
                {
                    await _conditionalTaskStorage.DeleteTaskAsync(task);
                    await TaskToastHandler.ShowTaskDeletedToastAsync();
                }
            });

            PerformOneTimeTaskCommand = new(async (task) =>
            {
                if (!CanInteractWithTask)
                    return;

                await PerformOneTimeTaskAsync(task);
            });

            EditOneTimeTaskCommand = new(async (task) =>
            {
                if (!CanInteractWithTask)
                    return;

                var parameters = new ShellNavigationQueryParameters()
                {
                    [nameof(OneTimeTask)] = task
                };

                await PageNavigator.GoToTaskEditPageAsync(parameters);
            });

            ResetOneTimeTaskCommand = new(async (task) =>
            {
                if (!CanInteractWithTask)
                    return;

                await ResetOneTimeTaskAsync(task);
            });

            DeleteOneTimeTaskCommand = new(async (task) =>
            {
                if (!CanInteractWithTask)
                    return;

                bool shouldDelete = await PopupHandler.ShowTaskDeletePopupAsync(task.ActionName);

                if (shouldDelete)
                {
                    await _oneTimeTaskStorage.DeleteTaskAsync(task);
                    await TaskToastHandler.ShowTaskDeletedToastAsync();
                }
            });

            AddTaskCommand = new Command(
            execute: async () =>
            {
                CanInteractWithTask = false;

                await PageNavigator.GoToTaskEditPageAsync();
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
            if (OneTimeTasksLoader.IsNotStarted)
                OneTimeTasksLoader.Load(_ => _oneTimeTaskStorage.LoadTasks());

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

        private async Task PerformOneTimeTaskAsync(OneTimeTask task)
        {
            if (!CanPerformTask(task)) return;

            await _oneTimeTaskStorage.PerformTaskAsync(task);
        }

        private async Task PerformСonditionalTaskAsync(ConditionalTask task)
        {
            if (!CanPerformTask(task)) return;

            await _conditionalTaskStorage.PerformTaskAsync(task);
        }

        private async Task ResetOneTimeTaskAsync(OneTimeTask task)
        {
            if (task == null || task.RepeatCount == 0) return;

            await _oneTimeTaskStorage.ResetTaskAsync(task);
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
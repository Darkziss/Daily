using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Tasks;
using Daily.Toasts;
using Daily.Navigation;

namespace Daily.ViewModels
{
    public partial class TaskEditPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private int _selectedTaskSegmentIndex = 0;
        
        [ObservableProperty] private string _actionName = string.Empty;

        [ObservableProperty] private int _priorityIndex = 0;
        [ObservableProperty] private int _repeatTimePeriodIndex = 0;

        [ObservableProperty] private int _targetRepeatCount = 1;

        [ObservableProperty] private string _note = string.Empty;

        [ObservableProperty] private bool _isEditMode = false;

        [ObservableProperty] private bool _isRecurringTaskMode = false;
        [ObservableProperty] private bool _isCreatingNewTask = false;

        private TaskBase? _currentTask = null;

        private readonly OneTimeTaskStorage _oneTimeTaskStorage;
        private readonly RecurringTaskStorage _recurringTaskStorage;

        public Command ChangeViewCommand { get; }

        public Command MakeTaskReady { get; }

        private bool CanCreateTask => !IsCreatingNewTask && !string.IsNullOrWhiteSpace(_actionName);

        private const int RecurringTaskSegmentIndex = 1;

        public TaskEditPageViewModel(OneTimeTaskStorage oneTimeTaskStorage, 
            RecurringTaskStorage recurringTaskStorage)
        {
            _oneTimeTaskStorage = oneTimeTaskStorage;
            _recurringTaskStorage = recurringTaskStorage;

            ChangeViewCommand = new Command(
            execute: () =>
            {
                IsRecurringTaskMode = SelectedTaskSegmentIndex == RecurringTaskSegmentIndex;
            },
            canExecute: () => !IsCreatingNewTask);

            MakeTaskReady = new Command(
            execute: async () =>
            {
                IsCreatingNewTask = true;

                async Task CreateTaskAsync()
                {
                    if (IsRecurringTaskMode) await CreateRecurringTaskAsync();
                    else await CreateOneTimeTaskAsync();
                }

                async Task EditTaskAsync()
                {
                    if (IsRecurringTaskMode) await EditRecurringTaskAsync();
                    else await EditOneTimeTaskAsync();
                }

                if (IsEditMode) await EditTaskAsync();
                else await CreateTaskAsync();

                await PageNavigator.ReturnToPreviousPageAsync();

                IsCreatingNewTask = false;
            },
            canExecute: () => CanCreateTask);

            PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(ActionName))
                {
                    MakeTaskReady.ChangeCanExecute();
                }
                else if (args.PropertyName == nameof(IsCreatingNewTask))
                {
                    ChangeViewCommand.ChangeCanExecute();
                }
            };
        }

        public void PrepareViewForEdit(OneTimeTask task)
        {
            _currentTask = task;
            
            IsEditMode = true;
            IsRecurringTaskMode = false;

            SelectedTaskSegmentIndex = 0;

            ActionName = task.ActionName;

            PriorityIndex = (int)task.Priority;
            RepeatTimePeriodIndex = 0;

            TargetRepeatCount = task.TargetRepeatCount;

            Note = task.Note;
        }

        public void PrepareViewForEdit(RecurringTask task)
        {
            _currentTask = task;
            
            IsEditMode = true;
            IsRecurringTaskMode = true;

            SelectedTaskSegmentIndex = 1;

            ActionName = task.ActionName;

            PriorityIndex = 0;
            RepeatTimePeriodIndex = (int)task.RepeatTimePeriod;

            TargetRepeatCount = task.TargetRepeatCount;

            Note = task.Note;
        }

        public void ResetView()
        {
            _currentTask = null;

            IsEditMode = false;

            SelectedTaskSegmentIndex = 0;
            
            ActionName = string.Empty;

            PriorityIndex = 0;
            RepeatTimePeriodIndex = 0;

            TargetRepeatCount = 1;

            Note = string.Empty;
        }

        private async Task CreateOneTimeTaskAsync()
        {
            if (_oneTimeTaskStorage.IsTasksFull)
            {
                await TaskToastHandler.ShowOneTimeTasksFullToastAsync();
                return;
            }
            
            TaskPriority priority = (TaskPriority)PriorityIndex;
            OneTimeTask task = new OneTimeTask(ActionName, 0, TargetRepeatCount, priority, Note);

            bool isCreated = await _oneTimeTaskStorage.TryAddTaskAsync(task);

            if (isCreated) await TaskToastHandler.ShowTaskCreatedToastAsync();
            else await TaskToastHandler.ShowTaskErrorToastAsync();
        }

        private async Task CreateRecurringTaskAsync()
        {
            if (_recurringTaskStorage.IsTasksFull)
            {
                await TaskToastHandler.ShowRecurringTasksFullToastAsync();
                return;
            }
            
            TaskRepeatTimePeriod repeatTimePeriod = (TaskRepeatTimePeriod)RepeatTimePeriodIndex;
            RecurringTask task = new RecurringTask(ActionName, 0, TargetRepeatCount, repeatTimePeriod, Note);

            bool isCreated = await _recurringTaskStorage.TryAddTaskAsync(task);

            if (isCreated) await TaskToastHandler.ShowTaskCreatedToastAsync();
            else await TaskToastHandler.ShowTaskErrorToastAsync();
        }

        private async Task EditOneTimeTaskAsync()
        {
            OneTimeTask oldTask = (OneTimeTask)_currentTask!;

            TaskPriority priority = (TaskPriority)PriorityIndex;
            OneTimeTask newTask = new OneTimeTask(ActionName, oldTask.RepeatCount, TargetRepeatCount, priority, Note);

            bool isEdited = await _oneTimeTaskStorage.TryEditTaskAsync(oldTask, newTask);

            if (isEdited) await TaskToastHandler.ShowTaskEditedToastAsync();
            else await TaskToastHandler.ShowTaskErrorToastAsync();
        }

        private async Task EditRecurringTaskAsync()
        {
            RecurringTask oldTask = (RecurringTask)_currentTask!;

            TaskRepeatTimePeriod repeatTimePeriod = (TaskRepeatTimePeriod)RepeatTimePeriodIndex;
            RecurringTask newTask = new RecurringTask(ActionName, oldTask.RepeatCount, TargetRepeatCount, repeatTimePeriod, Note);

            bool isEdited = await _recurringTaskStorage.TryEditTaskAsync(oldTask, newTask);

            if (isEdited) await TaskToastHandler.ShowTaskEditedToastAsync();
            else await TaskToastHandler.ShowTaskErrorToastAsync();
        }
    }
}
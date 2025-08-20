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

        [ObservableProperty] private bool _isConditionalTaskMode = false;
        [ObservableProperty] private bool _isCreatingNewTask = false;

        private TaskBase? _currentTask = null;

        private readonly GeneralTaskStorage _generalTaskStorage;
        private readonly ConditionalTaskStorage _conditionalTaskStorage;

        public Command ChangeViewCommand { get; }

        public Command MakeTaskReady { get; }

        private bool CanCreateTask => !IsCreatingNewTask && !string.IsNullOrWhiteSpace(_actionName);

        public TaskEditPageViewModel(GeneralTaskStorage generalTaskStorage, 
            ConditionalTaskStorage conditionalTaskStorage)
        {
            _generalTaskStorage = generalTaskStorage;
            _conditionalTaskStorage = conditionalTaskStorage;

            ChangeViewCommand = new Command(
            execute: () =>
            {
                const int conditionalTaskSegmentIndex = 1;

                IsConditionalTaskMode = SelectedTaskSegmentIndex == conditionalTaskSegmentIndex;
            },
            canExecute: () => !IsCreatingNewTask);

            MakeTaskReady = new Command(
            execute: async () =>
            {
                IsCreatingNewTask = true;

                async Task CreateTaskAsync()
                {
                    if (IsConditionalTaskMode) await CreateConditionalTaskAsync();
                    else await CreateGeneralTaskAsync();
                }

                async Task EditTaskAsync()
                {
                    if (IsConditionalTaskMode) await EditConditionalTaskAsync();
                    else await EditGeneralTaskAsync();
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

        public void PrepareViewForEdit(GeneralTask task)
        {
            _currentTask = task;
            
            IsEditMode = true;
            IsConditionalTaskMode = false;

            SelectedTaskSegmentIndex = 0;

            ActionName = task.ActionName;

            PriorityIndex = (int)task.Priority;
            RepeatTimePeriodIndex = 0;

            TargetRepeatCount = task.TargetRepeatCount;

            Note = task.Note;
        }

        public void PrepareViewForEdit(ConditionalTask task)
        {
            _currentTask = task;
            
            IsEditMode = true;
            IsConditionalTaskMode = true;

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

        private async Task CreateGeneralTaskAsync()
        {
            if (_generalTaskStorage.IsTasksFull)
            {
                await TaskToastHandler.ShowGeneralTasksFullToastAsync();
                return;
            }
            
            TaskPriority priority = (TaskPriority)PriorityIndex;
            GeneralTask task = new GeneralTask(ActionName, 0, TargetRepeatCount, priority, Note);

            bool isCreated = await _generalTaskStorage.TryAddTaskAsync(task);

            if (isCreated) await TaskToastHandler.ShowTaskCreatedToastAsync();
            else await TaskToastHandler.ShowTaskErrorToastAsync();
        }

        private async Task CreateConditionalTaskAsync()
        {
            if (_conditionalTaskStorage.IsTasksFull)
            {
                await TaskToastHandler.ShowConditionalTasksFullToastAsync();
                return;
            }
            
            TaskRepeatTimePeriod repeatTimePeriod = (TaskRepeatTimePeriod)RepeatTimePeriodIndex;
            ConditionalTask task = new ConditionalTask(ActionName, 0, TargetRepeatCount, repeatTimePeriod, Note);

            bool isCreated = await _conditionalTaskStorage.TryAddTaskAsync(task);

            if (isCreated) await TaskToastHandler.ShowTaskCreatedToastAsync();
            else await TaskToastHandler.ShowTaskErrorToastAsync();
        }

        private async Task EditGeneralTaskAsync()
        {
            GeneralTask oldTask = (GeneralTask)_currentTask!;

            TaskPriority priority = (TaskPriority)PriorityIndex;
            GeneralTask newTask = new GeneralTask(ActionName, oldTask.RepeatCount, TargetRepeatCount, priority, Note);

            bool isEdited = await _generalTaskStorage.TryEditTaskAsync(oldTask, newTask);

            if (isEdited) await TaskToastHandler.ShowTaskEditedToastAsync();
            else await TaskToastHandler.ShowTaskErrorToastAsync();
        }

        private async Task EditConditionalTaskAsync()
        {
            ConditionalTask oldTask = (ConditionalTask)_currentTask!;

            TaskRepeatTimePeriod repeatTimePeriod = (TaskRepeatTimePeriod)RepeatTimePeriodIndex;
            ConditionalTask newTask = new ConditionalTask(ActionName, oldTask.RepeatCount, TargetRepeatCount, repeatTimePeriod, Note);

            bool isEdited = await _conditionalTaskStorage.TryEditTaskAsync(oldTask, newTask);

            if (isEdited) await TaskToastHandler.ShowTaskEditedToastAsync();
            else await TaskToastHandler.ShowTaskErrorToastAsync();
        }
    }
}
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

        [ObservableProperty] private int _completionTime = 0;
        [ObservableProperty] private string _note = string.Empty;

        [ObservableProperty] private bool _isEditMode = false;

        [ObservableProperty] private bool _isConditionalTaskMode = false;
        [ObservableProperty] private bool _isCreatingNewTask = false;

        private TaskBase? _currentTask = null;

        private readonly TaskStorage _taskStorage;

        public Command ChangeViewCommand { get; }

        public Command CreateTaskCommand { get; }
        public Command EditTaskCommand { get; }

        private bool CanCreateTask => !IsCreatingNewTask && !string.IsNullOrWhiteSpace(_actionName);

        public TaskEditPageViewModel(TaskStorage taskStorage)
        {
            _taskStorage = taskStorage;

            ChangeViewCommand = new Command(
            execute: () =>
            {
                const int conditionalTaskSegmentIndex = 1;

                IsConditionalTaskMode = SelectedTaskSegmentIndex == conditionalTaskSegmentIndex;
            },
            canExecute: () => !IsCreatingNewTask);

            CreateTaskCommand = new Command(
            execute: async () =>
            {
                IsCreatingNewTask = true;

                if (IsConditionalTaskMode) await CreateConditionalTaskAsync();
                else await CreateGeneralTaskAsync();
                
                await PageNavigator.RouteToPreviousPage();

                IsCreatingNewTask = false;
            },
            canExecute: () => CanCreateTask);

            EditTaskCommand = new Command(
            execute: async () =>
            {
                if (_currentTask == null) return;

                IsCreatingNewTask = true;

                await EditGeneralTaskAsync();

                await PageNavigator.RouteToPreviousPage();

                IsCreatingNewTask = false;
            });

            PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(ActionName))
                {
                    CreateTaskCommand.ChangeCanExecute();
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

            SelectedTaskSegmentIndex = 0;

            ActionName = task.ActionName;

            PriorityIndex = (int)task.Priority;
            RepeatTimePeriodIndex = 0;

            TargetRepeatCount = task.TargetRepeatCount;

            CompletionTime = 0;
            Note = string.Empty;
        }

        public void PrepareViewForEdit(СonditionalTask task)
        {
            IsEditMode = true;

            SelectedTaskSegmentIndex = 1;

            ActionName = task.ActionName;

            PriorityIndex = 0;
            RepeatTimePeriodIndex = (int)task.RepeatTimePeriod;

            TargetRepeatCount = task.TargetRepeatCount;

            CompletionTime = task.CompletionTime;
            Note = task.Note;
        }

        public void ResetView()
        {
            IsEditMode = false;

            SelectedTaskSegmentIndex = 0;
            
            ActionName = string.Empty;

            PriorityIndex = 0;
            RepeatTimePeriodIndex = 0;

            TargetRepeatCount = 1;

            CompletionTime = 0;
            Note = string.Empty;
        }

        private async Task CreateGeneralTaskAsync()
        {
            if (_taskStorage.IsGeneralTasksFull)
            {
                await TaskToastHandler.ShowGeneralTasksFullToastAsync();
                return;
            }
            
            TaskPriority priority = (TaskPriority)PriorityIndex;

            await _taskStorage.CreateGeneralTaskAsync(ActionName, TargetRepeatCount, priority);
            await TaskToastHandler.ShowTaskCreatedToastAsync();
        }

        private async Task CreateConditionalTaskAsync()
        {
            if (_taskStorage.IsConditionalTasksFull) await TaskToastHandler.ShowConditionalTasksFullToastAsync();
            else
            {
                TaskRepeatTimePeriod repeatTimePeriod = (TaskRepeatTimePeriod)RepeatTimePeriodIndex;

                await _taskStorage.CreateConditionalTaskAsync(ActionName, TargetRepeatCount, repeatTimePeriod, CompletionTime, Note);
                await TaskToastHandler.ShowTaskCreatedToastAsync();
            }
        }

        private async Task EditGeneralTaskAsync()
        {
            TaskPriority priority = (TaskPriority)PriorityIndex;

            GeneralTask task = new GeneralTask(ActionName, TargetRepeatCount, priority);

            await _taskStorage.EditGeneralTaskAsync(_currentTask!, task);
            await TaskToastHandler.ShowTaskEditedToastAsync();
        }
    }
}
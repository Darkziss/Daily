using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Alerts;
using Daily.Tasks;
using System.Diagnostics;

namespace Daily.ViewModels
{
    public partial class TaskEditPageViewModel : ObservableObject, IPrepareView
    {
        [ObservableProperty] private int _selectedTaskSegmentIndex = 0;
        
        [ObservableProperty] private string _actionName = string.Empty;

        [ObservableProperty] private int _priorityIndex = 0;
        [ObservableProperty] private int _repeatTimePeriodIndex = 0;

        [ObservableProperty] private int _targetRepeatCount = 1;

        [ObservableProperty] private int _completionTime = 0;

        [ObservableProperty] private bool _isConditionalTaskMode = false;
        [ObservableProperty] private bool _isCreatingNewTask = false;

        private readonly TaskStorage _taskStorage;

        private readonly IToast _taskCreatedToast = Toast.Make(taskCreatedToastMessage, 
            toastDuration, toastTextSize);

        private readonly IToast _generalTasksFullToast = Toast.Make(generalTasksFullToastMessage, 
            toastDuration, toastTextSize);
        private readonly IToast _conditionalTasksFullToast = Toast.Make(conditionalTasksFullToastMessage,
            toastDuration, toastTextSize);

        public Command ChangeViewCommand { get; }

        public Command CreateTaskCommand { get; }

        private bool CanCreateTask => !IsCreatingNewTask && !string.IsNullOrWhiteSpace(_actionName);

        private const string taskCreatedToastMessage = "Задача была успешно создана";
        private const string generalTasksFullToastMessage = "Ошибка: Уже создано максимум основных задач";
        private const string conditionalTasksFullToastMessage = "Ошибка: Уже создано максимум условных задач";

        private const ToastDuration toastDuration = ToastDuration.Long;
        private const double toastTextSize = 16d;

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
                
                await PageRouter.RouteToPrevious();

                IsCreatingNewTask = false;
            },
            canExecute: () => CanCreateTask);

            PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(ActionName))
                {
                    CreateTaskCommand.ChangeCanExecute();
                    return;
                }
                else if (args.PropertyName == nameof(IsCreatingNewTask))
                {
                    ChangeViewCommand.ChangeCanExecute();
                    return;
                }
            };
        }

        public void PrepareView()
        {
            ActionName = string.Empty;
            
            PriorityIndex = 0;
            RepeatTimePeriodIndex = 0;

            TargetRepeatCount = 1;

            CompletionTime = 0;
        }

        private async Task CreateGeneralTaskAsync()
        {
            if (_taskStorage.IsGeneralTasksFull) await _generalTasksFullToast.Show();
            else
            {
                TaskPriority priority = (TaskPriority)PriorityIndex;

                await _taskStorage.CreateGeneralTaskAsync(ActionName, priority, TargetRepeatCount);
                await _taskCreatedToast.Show();
            }
        }

        private async Task CreateConditionalTaskAsync()
        {
            if (_taskStorage.IsConditionalTasksFull) await _conditionalTasksFullToast.Show();
            else
            {
                TaskRepeatTimePeriod repeatTimePeriod = (TaskRepeatTimePeriod)RepeatTimePeriodIndex;

                await _taskStorage.CreateConditionalTaskAsync(ActionName, TargetRepeatCount, repeatTimePeriod, CompletionTime);
                await _taskCreatedToast.Show();
            }
        }
    }
}
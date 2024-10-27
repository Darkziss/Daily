using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Alerts;
using Daily.Tasks;

namespace Daily.ViewModels
{
    public partial class TaskEditPageViewModel : ObservableObject
    {
        [ObservableProperty] private string _actionName = string.Empty;
        [ObservableProperty] private int _selectedPriorityIndex = 0;
        [ObservableProperty] private int _targetRepeatCount = 1;

        [ObservableProperty] private bool _isCreatingNewTask = false;

        private readonly TaskStorage _taskStorage;

        private readonly IToast _taskCreatedToast = Toast.Make(taskCreatedToastMessage, 
            defaultToastDuration, defaultToastTextSize);
        private readonly IToast _generalTasksFullToast = Toast.Make(generalTasksFullToastMessage, 
            defaultToastDuration, defaultToastTextSize);

        public Command CreateGeneralTaskCommand { get; }

        private bool CanCreateTask => !IsCreatingNewTask && !string.IsNullOrWhiteSpace(_actionName);

        private const string taskCreatedToastMessage = "Задача была успешно создана";
        private const string generalTasksFullToastMessage = "Уже создано максимум основных задач";

        private const ToastDuration defaultToastDuration = ToastDuration.Long;
        private const double defaultToastTextSize = 16d;

        public TaskEditPageViewModel(TaskStorage taskStorage)
        {
            _taskStorage = taskStorage;

            CreateGeneralTaskCommand = new Command(
            execute: async () =>
            {
                IsCreatingNewTask = true;
                
                if (taskStorage.IsGeneralTasksFull) await _generalTasksFullToast.Show();
                else
                {
                    TaskPriority priority = (TaskPriority)SelectedPriorityIndex;

                    await _taskStorage.CreateGeneralTaskAsync(ActionName, priority, TargetRepeatCount);
                    await _taskCreatedToast.Show();
                }

                await PageRouter.RouteToPrevious();

                IsCreatingNewTask = false;

                ResetToDefault();
            },
            canExecute: () => CanCreateTask);
        }

        private void ResetToDefault()
        {
            ActionName = string.Empty;
            SelectedPriorityIndex = 0;
            TargetRepeatCount = 1;
        }
    }
}
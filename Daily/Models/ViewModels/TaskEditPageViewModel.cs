using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
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

        public bool IsActionNameEntryEmpty { get; set; }

        public Command CreateGeneralTaskCommand { get; }

        private const int loadingDelay = 1500;

        public TaskEditPageViewModel(TaskStorage taskStorage)
        {
            _taskStorage = taskStorage;

            CreateGeneralTaskCommand = new Command(
            execute: async () =>
            {
                IsCreatingNewTask = true;
                
                TaskPriority priority = (TaskPriority)SelectedPriorityIndex;

                Debug.WriteLine($"TargetRepeatCount: {TargetRepeatCount}");

                await _taskStorage.CreateGeneralTaskAsync(ActionName, priority, TargetRepeatCount);
                await Task.Delay(loadingDelay);

                IsCreatingNewTask = false;

                ResetToDefault();
            },
            canExecute: () => !IsCreatingNewTask && !IsActionNameEntryEmpty);
        }

        private void ResetToDefault()
        {
            ActionName = string.Empty;
            SelectedPriorityIndex = 0;
            TargetRepeatCount = 1;
        }
    }
}
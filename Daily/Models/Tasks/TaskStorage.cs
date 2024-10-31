using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Tasks
{
    public class TaskStorage
    {
        private readonly ObservableCollection<GeneralTask> _generalTasks;
        private readonly DataProvider _dataProvider;

        public ObservableCollection<GeneralTask> GeneralTasks => _generalTasks;

        public bool IsGeneralTasksFull => _generalTasks.Count == maxGeneralTaskCount;

        private const int minRepeatCount = 1;
        private const int maxRepeatCount = 3;

        private const int maxGeneralTaskCount = 10;

        private const string maxGeneralTasksExceptionText = "Already created max amount of general tasks";
        private const string taskIndexOutOfRangeExceptionText = "Task index is out of range";

        public TaskStorage(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            if (dataProvider.GeneralTasks == null) _generalTasks = new ObservableCollection<GeneralTask>();
            else _generalTasks = new ObservableCollection<GeneralTask>(dataProvider.GeneralTasks);
        }

        public async Task CreateGeneralTaskAsync(string action, TaskPriority priority, int targetRepeatCount)
        {
            if (IsGeneralTasksFull) throw new Exception(maxGeneralTasksExceptionText);

            GeneralTask task = new GeneralTask(action, priority, targetRepeatCount);

            if (!ValidateGeneralTask(task)) return;

            _generalTasks.Add(task);

            await _dataProvider.SaveGeneralTasksAsync(_generalTasks);
        }

        public async Task PerformGeneralTaskByIndexAsync(int index)
        {
            if (index < 0 || index >= _generalTasks.Count) throw new IndexOutOfRangeException(taskIndexOutOfRangeExceptionText);

            _generalTasks[index].Perform();

            await _dataProvider.SaveGeneralTasksAsync(_generalTasks);
        }

        private bool ValidateGeneralTask(GeneralTask task)
        {
            if (string.IsNullOrWhiteSpace(task.ActionName)) return false;
            else if (task.TargetRepeatCount < minRepeatCount || task.TargetRepeatCount > maxRepeatCount) return false;
            else return true;
        }
    }
}
using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Tasks
{
    public class TaskStorage
    {
        private readonly DataProvider _dataProvider;

        public ObservableCollection<GeneralTask> GeneralTasks { get; }

        public bool IsGeneralTasksFull => GeneralTasks.Count == maxGeneralTaskCount;

        private const int minRepeatCount = 1;
        private const int maxRepeatCount = 3;

        private const int maxGeneralTaskCount = 10;

        private const string maxGeneralTasksExceptionText = "Already created max amount of general tasks";
        private const string taskIndexOutOfRangeExceptionText = "Task index is out of range";
        private const string taskIsNotOnListExceptionText = "Task is not on the list";

        public TaskStorage(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            if (dataProvider.GeneralTasks == null) GeneralTasks = new ObservableCollection<GeneralTask>();
            else GeneralTasks = new ObservableCollection<GeneralTask>(dataProvider.GeneralTasks);
        }

        public async Task CreateGeneralTaskAsync(string action, TaskPriority priority, int targetRepeatCount)
        {
            if (IsGeneralTasksFull) throw new Exception(maxGeneralTasksExceptionText);

            GeneralTask task = new GeneralTask(action, priority, targetRepeatCount);

            if (!ValidateGeneralTask(task)) return;

            GeneralTasks.Add(task);

            await _dataProvider.SaveGeneralTasksAsync(GeneralTasks);
        }

        public async Task PerformGeneralTaskAsync(GeneralTask task)
        {
            if (task == null) return;
            else if (!GeneralTasks.Contains(task)) throw new ArgumentException(taskIsNotOnListExceptionText);

            task.Perform();

            await _dataProvider.SaveGeneralTasksAsync(GeneralTasks);
        }

        private bool ValidateGeneralTask(GeneralTask task)
        {
            if (string.IsNullOrWhiteSpace(task.ActionName)) return false;
            else if (task.TargetRepeatCount < minRepeatCount || task.TargetRepeatCount > maxRepeatCount) return false;
            else return true;
        }
    }
}
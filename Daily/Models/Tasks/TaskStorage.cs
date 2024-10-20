using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Tasks
{
    public class TaskStorage
    {
        private readonly ObservableCollection<GeneralTask> _generalTasks;
        private readonly DataProvider _dataProvider;

        public ObservableCollection<GeneralTask> GeneralTasks => _generalTasks;

        private bool IsGeneralTasksFull => _generalTasks.Count == maxGeneralTaskCount;

        private const int minRepeatCount = 1;
        private const int maxRepeatCount = 3;

        private const int maxGeneralTaskCount = 10;

        public TaskStorage(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            if (dataProvider.GeneralTasks == null) _generalTasks = new ObservableCollection<GeneralTask>();
            else _generalTasks = new ObservableCollection<GeneralTask>(dataProvider.GeneralTasks);
        }

        public async Task CreateGeneralTaskAsync(string action, TaskPriority priority, int repeatCount)
        {
            if (ValidateRepeatCount(repeatCount) || IsGeneralTasksFull || string.IsNullOrWhiteSpace(action)) return;

            GeneralTask task = new GeneralTask(action, priority, repeatCount);

            _generalTasks.Add(task);

            List<GeneralTask> tasks = _generalTasks.ToList();
            await _dataProvider.SaveGeneralTasksAsync(tasks);
        }

        private bool ValidateRepeatCount(int count) => !(count < minRepeatCount || count > maxRepeatCount);
    }
}
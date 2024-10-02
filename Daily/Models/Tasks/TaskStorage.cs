using Daily.Data;

namespace Daily.Tasks
{
    public class TaskStorage
    {
        private readonly List<GeneralTask> _generalTasks = new List<GeneralTask>(maxGeneralTaskCount);

        private readonly DataProvider _dataProvider;

        private const int maxGeneralTaskCount = 10;

        public TaskStorage(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            _generalTasks = dataProvider.GetGeneralTasks();
        }

        public async Task CreateGeneralTaskAsync(string action, TaskPriority priority, int repeatCount)
        {
            if (repeatCount < 1 || _generalTasks.Count == maxGeneralTaskCount || string.IsNullOrWhiteSpace(action)) return;

            GeneralTask task = new GeneralTask(action, priority, repeatCount);

            _generalTasks.Add(task);

            await _dataProvider.SaveGeneralTasks(_generalTasks);
        }
    }
}
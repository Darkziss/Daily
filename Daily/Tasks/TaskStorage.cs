using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Tasks
{
    public class TaskStorage
    {
        private readonly DataProvider _dataProvider;

        public ObservableCollection<GeneralTask> GeneralTasks { get; private set; }
        public ObservableCollection<СonditionalTask> СonditionalTasks { get; private set; }

        public bool IsGeneralTasksFull => GeneralTasks.Count == maxGeneralTaskCount;
        public bool IsConditionalTasksFull => СonditionalTasks.Count == maxConditionalTaskCount;

        private const int maxGeneralTaskCount = 10;
        private const int maxConditionalTaskCount = 5;

        private const string maxGeneralTasksExceptionText = "Already created max amount of general tasks";
        private const string maxConditionalTasksExceptionText = "Already created max amount of conditional tasks";

        private const string invalidTaskExceptionText = "Invalid task";
        private const string taskIsNotOnListExceptionText = "Task is not on the list";

        public TaskStorage(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            if (dataProvider.GeneralTasks == null) GeneralTasks = new ObservableCollection<GeneralTask>();
            else GeneralTasks = new ObservableCollection<GeneralTask>(dataProvider.GeneralTasks);

            if (dataProvider.СonditionalTasks == null) СonditionalTasks = new ObservableCollection<СonditionalTask>();
            else СonditionalTasks = new ObservableCollection<СonditionalTask>(dataProvider.СonditionalTasks);
        }

        #region GeneralTasks

        public async Task CreateGeneralTaskAsync(string action, int targetRepeatCount, TaskPriority priority)
        {
            if (IsGeneralTasksFull) throw new Exception(maxGeneralTasksExceptionText);

            GeneralTask task = new GeneralTask(action, targetRepeatCount, priority);

            bool isValid = TaskValidator.ValidateTask(task);

            if (!isValid) return;

            GeneralTasks.Add(task);
            if (GeneralTasks.Count > 1) SortGeneralTasks();

            await _dataProvider.SaveGeneralTasksAsync(GeneralTasks);
        }

        public async Task EditGeneralTaskAsync(GeneralTask oldTask, GeneralTask newTask)
        {
            bool isValid = TaskValidator.ValidateTask(newTask);

            if (!isValid) return;

            int index = GeneralTasks.IndexOf(oldTask);

            if (index == -1) throw new Exception(taskIsNotOnListExceptionText);

            GeneralTasks.RemoveAt(index);

            GeneralTasks.Add(newTask);
            if (GeneralTasks.Count > 1) SortGeneralTasks();

            await _dataProvider.SaveGeneralTasksAsync(GeneralTasks);
        }

        public async Task PerformGeneralTaskAsync(GeneralTask task)
        {
            if (task == null) return;
            else if (!GeneralTasks.Contains(task)) throw new ArgumentException(taskIsNotOnListExceptionText);

            task.Perform();

            await _dataProvider.SaveGeneralTasksAsync(GeneralTasks);
        }

        public async Task ResetGeneralTaskAsync(GeneralTask task)
        {
            if (task == null) return;
            else if (!GeneralTasks.Contains(task)) throw new ArgumentException(taskIsNotOnListExceptionText);

            task.Reset();

            await _dataProvider.SaveGeneralTasksAsync(GeneralTasks);
        }

        public async Task DeleteGeneralTaskAsync(GeneralTask task)
        {
            if (task == null) return;

            int index = GeneralTasks.IndexOf(task);

            if (index == -1) throw new ArgumentException(taskIsNotOnListExceptionText);

            GeneralTasks.RemoveAt(index);

            await _dataProvider.SaveGeneralTasksAsync(GeneralTasks);
        }

        #endregion

        #region СonditionalTasks

        public async Task CreateConditionalTaskAsync(string action, int targetRepeatCount, TaskRepeatTimePeriod repeatTimePeriod, int minCompletionTime, string note)
        {
            if (IsConditionalTasksFull) throw new Exception(maxConditionalTasksExceptionText);

            СonditionalTask task = new СonditionalTask(action, targetRepeatCount, repeatTimePeriod, minCompletionTime, note);

            bool isValid = TaskValidator.ValidateСonditionalTask(task);

            if (!isValid) return;

            СonditionalTasks.Add(task);
            if (СonditionalTasks.Count > 1) SortConditionalTasks();

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);
        }

        public async Task EditConditionalTaskAsync(СonditionalTask oldTask, СonditionalTask newTask)
        {
            bool isValid = TaskValidator.ValidateСonditionalTask(newTask);

            if (!isValid) return;

            int index = СonditionalTasks.IndexOf(oldTask);

            if (index == -1) throw new Exception(taskIsNotOnListExceptionText);

            СonditionalTasks.RemoveAt(index);

            СonditionalTasks.Add(newTask);
            if (СonditionalTasks.Count > 1) SortConditionalTasks();

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);
        }

        public async Task PerformСonditionalTaskAsync(СonditionalTask task)
        {
            if (task == null) return;
            else if (!СonditionalTasks.Contains(task)) throw new ArgumentException(taskIsNotOnListExceptionText);

            task.Perform();

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);
        }

        public async Task ResetСonditionalTaskAsync(СonditionalTask task)
        {
            if (task == null) return;
            else if (!СonditionalTasks.Contains(task)) throw new ArgumentException(taskIsNotOnListExceptionText);

            task.Reset();

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);
        }

        public async Task DeleteConditionalTaskAsync(СonditionalTask task)
        {
            if (task == null) return;

            int index = СonditionalTasks.IndexOf(task);

            if (index == -1) throw new ArgumentException(taskIsNotOnListExceptionText);

            СonditionalTasks.RemoveAt(index);

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);
        }

        private void SortGeneralTasks()
        {
            var sorted = GeneralTasks.OrderBy(task => task.Priority)
                .ThenBy(task => task.ActionName)
                .ToList();

            GeneralTasks.Clear();

            foreach (GeneralTask task in sorted)
            {
                GeneralTasks.Add(task);
            }
        }

        private void SortConditionalTasks()
        {
            var sorted = СonditionalTasks.OrderByDescending(task => task.RepeatTimePeriod)
                .ThenBy(task => task.ActionName)
                .ToList();

            СonditionalTasks.Clear();

            foreach (СonditionalTask task in sorted)
            {
                СonditionalTasks.Add(task);
            }
        }

        #endregion
    }
}
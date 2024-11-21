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

        private bool ShouldSortGeneralTasks => GeneralTasks.Count > 1;
        private bool ShouldSortСonditionalTasks => СonditionalTasks.Count > 1;

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

        public async Task<bool> TryAddGeneralTaskAsync(GeneralTask task)
        {
            if (IsGeneralTasksFull) throw new Exception(maxGeneralTasksExceptionText);

            bool isValid = TaskValidator.ValidateTask(task);
            bool contains = GeneralTasks.Contains(task);

            if (!isValid || contains) return false;

            GeneralTasks.Add(task);
            if (ShouldSortGeneralTasks) SortGeneralTasks();

            await _dataProvider.SaveGeneralTasksAsync(GeneralTasks);

            return true;
        }

        public async Task<bool> TryEditGeneralTaskAsync(GeneralTask oldTask, GeneralTask newTask)
        {
            bool isValid = TaskValidator.ValidateTask(newTask);

            if (!isValid) return false;

            int index = GeneralTasks.IndexOf(oldTask);

            if (index == -1) throw new Exception(taskIsNotOnListExceptionText);

            GeneralTasks[index] = newTask;
            if (ShouldSortGeneralTasks) SortGeneralTasks();

            await _dataProvider.SaveGeneralTasksAsync(GeneralTasks);

            return true;
        }

        public async Task PerformGeneralTaskAsync(GeneralTask task)
        {
            if (task == null) throw new ArgumentNullException(invalidTaskExceptionText);
            else if (!GeneralTasks.Contains(task)) throw new ArgumentException(taskIsNotOnListExceptionText);

            task.Perform();

            await _dataProvider.SaveGeneralTasksAsync(GeneralTasks);
        }

        public async Task ResetGeneralTaskAsync(GeneralTask task)
        {
            if (task == null) throw new ArgumentNullException(invalidTaskExceptionText);
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

        private void SortGeneralTasks()
        {
            var sorted = GeneralTasks.OrderBy(task => task.Priority)
                .ThenBy(task => task.ActionName.Length)
                .ToList();

            GeneralTasks.Clear();

            foreach (GeneralTask task in sorted)
            {
                GeneralTasks.Add(task);
            }
        }

        #endregion

        #region СonditionalTasks

        public async Task<bool> TryAddСonditionalTaskAsync(СonditionalTask task)
        {
            if (IsConditionalTasksFull) throw new Exception(maxConditionalTasksExceptionText);

            bool isValid = TaskValidator.ValidateСonditionalTask(task);
            bool contains = СonditionalTasks.Contains(task);

            if (!isValid || contains) return false;

            СonditionalTasks.Add(task);
            if (ShouldSortСonditionalTasks) SortConditionalTasks();

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);

            return true;
        }

        public async Task<bool> TryEditСonditionalTaskAsync(СonditionalTask oldTask, СonditionalTask newTask)
        {
            bool isValid = TaskValidator.ValidateСonditionalTask(newTask);

            if (!isValid) return false;

            int index = СonditionalTasks.IndexOf(oldTask);

            if (index == -1) throw new Exception(taskIsNotOnListExceptionText);

            СonditionalTasks[index] = newTask;
            if (ShouldSortСonditionalTasks) SortConditionalTasks();

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);

            return true;
        }

        public async Task PerformСonditionalTaskAsync(СonditionalTask task)
        {
            if (task == null) throw new ArgumentNullException(invalidTaskExceptionText);
            else if (!СonditionalTasks.Contains(task)) throw new ArgumentException(taskIsNotOnListExceptionText);

            task.Perform();

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);
        }

        public async Task ResetСonditionalTaskAsync(СonditionalTask task)
        {
            if (task == null) throw new ArgumentNullException(invalidTaskExceptionText);
            else if (!СonditionalTasks.Contains(task)) throw new ArgumentException(taskIsNotOnListExceptionText);

            task.Reset();

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);
        }

        public async Task DeleteConditionalTaskAsync(СonditionalTask task)
        {
            if (task == null) throw new ArgumentNullException(invalidTaskExceptionText);

            int index = СonditionalTasks.IndexOf(task);

            if (index == -1) throw new ArgumentException(taskIsNotOnListExceptionText);

            СonditionalTasks.RemoveAt(index);

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);
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
using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Tasks
{
    public class TaskStorage
    {
        private readonly DataProvider _dataProvider;

        public ObservableCollection<GeneralTask> GeneralTasks { get; }
        public ObservableCollection<СonditionalTask> СonditionalTasks { get; }

        public bool IsGeneralTasksFull => GeneralTasks.Count == maxGeneralTaskCount;
        public bool IsConditionalTasksFull => СonditionalTasks.Count == maxConditionalTaskCount;

        private const int minRepeatCount = 1;
        private const int maxRepeatCount = 3;

        private const int maxGeneralTaskCount = 10;
        private const int maxConditionalTaskCount = 5;

        private const string maxGeneralTasksExceptionText = "Already created max amount of general tasks";
        private const string maxConditionalTasksExceptionText = "Already created max amount of conditional tasks";

        private const string taskIndexOutOfRangeExceptionText = "Task index is out of range";
        private const string taskIsNotOnListExceptionText = "Task is not on the list";

        public TaskStorage(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            if (dataProvider.GeneralTasks == null) GeneralTasks = new ObservableCollection<GeneralTask>();
            else GeneralTasks = new ObservableCollection<GeneralTask>(dataProvider.GeneralTasks);

            if (dataProvider.СonditionalTasks == null) СonditionalTasks = new ObservableCollection<СonditionalTask>();
            else СonditionalTasks = new ObservableCollection<СonditionalTask>(dataProvider.СonditionalTasks);
        }

        public async Task CreateGeneralTaskAsync(string action, int targetRepeatCount, TaskPriority priority)
        {
            if (IsGeneralTasksFull) throw new Exception(maxGeneralTasksExceptionText);

            GeneralTask task = new GeneralTask(action, targetRepeatCount, priority);

            bool isValid = TaskValidator.ValidateTask(task);

            if (!isValid) return;

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

        public async Task CreateConditionalTaskAsync(string action, int targetRepeatCount, TaskRepeatTimePeriod repeatTimePeriod, int minCompletionTime = 0)
        {
            if (IsConditionalTasksFull) throw new Exception(maxConditionalTasksExceptionText);

            СonditionalTask task = new СonditionalTask(action, targetRepeatCount, repeatTimePeriod, minCompletionTime);

            bool isValid = TaskValidator.ValidateСonditionalTask(task);

            if (!isValid) return;

            СonditionalTasks.Add(task);

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);
        }

        public async Task PerformСonditionalTaskAsync(СonditionalTask task)
        {
            if (task == null) return;
            else if (!СonditionalTasks.Contains(task)) throw new ArgumentException(taskIsNotOnListExceptionText);

            task.Perform();

            await _dataProvider.SaveConditionalTasksAsync(СonditionalTasks);
        }

        private bool ValidateTask(TaskBase task)
        {
            if (string.IsNullOrWhiteSpace(task.ActionName)) return false;
            else if (task.TargetRepeatCount < minRepeatCount || task.TargetRepeatCount > maxRepeatCount) return false;
            else return true;
        }

        private TimeSpan GetRepeatTimePeriod(TaskRepeatTimePeriod period)
        {
            switch (period)
            {
                case TaskRepeatTimePeriod.Day:
                    return TimeSpan.FromDays(1);
                case TaskRepeatTimePeriod.Week:
                    return TimeSpan.FromDays(7);
                case TaskRepeatTimePeriod.Month:
                    return GetDaysInCurrentMonth();
                default:
                    throw new ArgumentOutOfRangeException();
            }

            TimeSpan GetDaysInCurrentMonth()
            {
                DateTime now = DateTime.Now;
                int days = DateTime.DaysInMonth(now.Year, now.Month);

                return TimeSpan.FromDays(days);
            }
        }
    }
}
using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Tasks
{
    public class ConditionalTaskStorage : TaskStorage<ConditionalTask>
    {
        public override ObservableCollection<ConditionalTask>? Tasks { get; protected set; }

        public override int MaxTaskCount { get; } = 10;

        public ConditionalTaskStorage(DataProvider dataProvider) : base(dataProvider)
        {
            if (_dataProvider.СonditionalTasks == null) Tasks = new ObservableCollection<СonditionalTask>();
            else Tasks = new ObservableCollection<СonditionalTask>(_dataProvider.СonditionalTasks);
        }

        public override async Task<bool> TryAddTaskAsync(ConditionalTask task)
        {
            if (IsTasksFull) return false;

            bool isValid = TaskValidator.ValidateСonditionalTask(task);
            bool contains = Tasks.Contains(task);

            if (!isValid || contains) return false;

            task.ActionName = task.ActionName.Trim();

            Tasks.Add(task);
            if (ShouldSort) SortTasks();

            await _dataProvider.SaveConditionalTasksAsync(Tasks);

            return true;
        }

        public override async Task<bool> TryEditTaskAsync(ConditionalTask oldTask, ConditionalTask newTask)
        {
            bool isValid = TaskValidator.ValidateСonditionalTask(newTask);

            if (!isValid) return false;

            int index = Tasks.IndexOf(oldTask);

            if (index == -1) return false;

            newTask.ActionName = newTask.ActionName.Trim();

            Tasks[index] = newTask;
            if (ShouldSort) SortTasks();

            await _dataProvider.SaveConditionalTasksAsync(Tasks);

            return true;
        }

        public override async Task PerformTaskAsync(ConditionalTask task)
        {
            if (IsNullOrUnknownTask(task)) return;

            task.Perform();

            await _dataProvider.SaveConditionalTasksAsync(Tasks);
        }

        public override async Task ResetTaskAsync(ConditionalTask task)
        {
            if (IsNullOrUnknownTask(task)) return;

            task.Reset();

            await _dataProvider.SaveConditionalTasksAsync(Tasks);
        }

        public override async Task DeleteTaskAsync(ConditionalTask task)
        {
            if (IsNullOrUnknownTask(task, out int index)) return;

            Tasks.RemoveAt(index);

            await _dataProvider.SaveConditionalTasksAsync(Tasks);
        }

        protected override void SortTasks()
        {
            var sorted = Tasks.OrderByDescending(task => task.RepeatTimePeriod)
                .ThenBy(task => task.ActionName.Length)
                .ToList();

            Tasks.Clear();

            foreach (ConditionalTask task in sorted)
            {
                Tasks.Add(task);
            }
        }
    }
}

using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Tasks
{
    public class GeneralTaskStorage : TaskStorage<GeneralTask>
    {
        public override ObservableCollection<GeneralTask> Tasks { get; protected set; }

        public override int MaxTaskCount { get; } = 15;

        public GeneralTaskStorage(DataProvider dataProvider) : base(dataProvider)
        {
            if (_dataProvider.GeneralTasks == null) Tasks = new ObservableCollection<GeneralTask>();
            else Tasks = new ObservableCollection<GeneralTask>(_dataProvider.GeneralTasks);
        }

        public override async Task<bool> TryAddTaskAsync(GeneralTask task)
        {
            if (IsTasksFull) return false;

            bool isValid = TaskValidator.ValidateTask(task);
            bool contains = Tasks.Contains(task);

            if (!isValid || contains) return false;

            Tasks.Add(task);
            if (ShouldSort) SortTasks();

            await _dataProvider.SaveGeneralTasksAsync(Tasks);

            return true;
        }

        public override async Task<bool> TryEditTaskAsync(GeneralTask oldTask, GeneralTask newTask)
        {
            bool isValid = TaskValidator.ValidateTask(newTask);

            if (!isValid) return false;

            int index = Tasks.IndexOf(oldTask);

            if (index == -1) return false;

            Tasks[index] = newTask;
            if (ShouldSort) SortTasks();

            await _dataProvider.SaveGeneralTasksAsync(Tasks);
            return true;
        }

        public override async Task PerformTaskAsync(GeneralTask task)
        {
            if (IsNullOrUnknownTask(task)) return;

            task.Perform();

            await _dataProvider.SaveGeneralTasksAsync(Tasks);
        }

        public override async Task ResetTaskAsync(GeneralTask task)
        {
            if (IsNullOrUnknownTask(task)) return;

            task.Reset();

            await _dataProvider.SaveGeneralTasksAsync(Tasks);
        }

        public override async Task DeleteTaskAsync(GeneralTask task)
        {
            if (IsNullOrUnknownTask(task, out int index)) return;

            Tasks.RemoveAt(index);

            await _dataProvider.SaveGeneralTasksAsync(Tasks);
        }

        protected override void SortTasks()
        {
            var sorted = Tasks.OrderBy(task => task.Priority)
                .ThenBy(task => task.ActionName.Length)
                .ToList();

            Tasks.Clear();

            foreach (GeneralTask task in sorted)
            {
                Tasks.Add(task);
            }
        }
    }
}

using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Tasks
{
    public class RecurringTaskStorage : TaskStorage<RecurringTask>
    {
        public override ObservableCollection<RecurringTask>? Tasks { get; protected set; }

        public override int MaxTaskCount { get; } = 10;

        public RecurringTaskStorage(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public override async Task<ObservableCollection<RecurringTask>> LoadTasks()
        {
            IEnumerable<RecurringTask>? tasks = await _unitOfWork.RecurringTaskRepository.LoadAsync();

            Tasks = tasks == null ? new() : new(tasks);

            return Tasks;
        }

        public override async Task<bool> TryAddTaskAsync(RecurringTask task)
        {
            if (IsTasksFull) return false;

            bool isValid = TaskValidator.ValidateTask(task);
            bool contains = Tasks.Contains(task);

            if (!isValid || contains) return false;

            task.ActionName = task.ActionName.Trim();

            Tasks.Add(task);
            if (ShouldSort) SortTasks();

            await _unitOfWork.RecurringTaskRepository.SaveAsync(Tasks);

            return true;
        }

        public override async Task<bool> TryEditTaskAsync(RecurringTask oldTask, RecurringTask newTask)
        {
            bool isValid = TaskValidator.ValidateTask(newTask);

            if (!isValid) return false;

            int index = Tasks.IndexOf(oldTask);

            if (index == -1) return false;

            newTask.ActionName = newTask.ActionName.Trim();

            Tasks[index] = newTask;
            if (ShouldSort) SortTasks();

            await _unitOfWork.RecurringTaskRepository.SaveAsync(Tasks);

            return true;
        }

        public override async Task PerformTaskAsync(RecurringTask task)
        {
            if (IsNullOrUnknownTask(task)) return;

            task.Perform();

            await _unitOfWork.RecurringTaskRepository.SaveAsync(Tasks);

        }

        public override async Task ResetTaskAsync(RecurringTask task)
        {
            if (IsNullOrUnknownTask(task)) return;

            task.Reset();

            await _unitOfWork.RecurringTaskRepository.SaveAsync(Tasks);
        }

        public override async Task DeleteTaskAsync(RecurringTask task)
        {
            if (IsNullOrUnknownTask(task, out int index)) return;

            Tasks.RemoveAt(index);

            await _unitOfWork.RecurringTaskRepository.SaveAsync(Tasks);
        }

        protected override void SortTasks()
        {
            var sorted = Tasks.OrderBy(task => task.RepeatTimePeriod)
                .ThenBy(task => task.ActionName.Length)
                .ToList();

            Tasks.Clear();

            foreach (RecurringTask task in sorted)
            {
                Tasks.Add(task);
            }
        }
    }
}

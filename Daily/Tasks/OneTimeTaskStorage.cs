using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Tasks
{
    public class OneTimeTaskStorage : TaskStorage<OneTimeTask>
    {
        public override ObservableCollection<OneTimeTask>? Tasks { get; protected set; }

        public override int MaxTaskCount { get; } = 15;

        public OneTimeTaskStorage(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public override async Task<ObservableCollection<OneTimeTask>> LoadTasks()
        {
            IEnumerable<OneTimeTask>? tasks = await _unitOfWork.OneTimeTaskRepository.LoadAsync();

            Tasks = tasks == null ? new() : new(tasks);

            return Tasks;
        }

        public override async Task<bool> TryAddTaskAsync(OneTimeTask task)
        {
            if (IsTasksFull) return false;

            bool isValid = TaskValidator.ValidateTask(task);
            bool contains = Tasks.Contains(task);

            if (!isValid || contains) return false;

            task.ActionName = task.ActionName.Trim();

            Tasks.Add(task);
            if (ShouldSort) SortTasks();

            await _unitOfWork.OneTimeTaskRepository.SaveAsync(Tasks);

            return true;
        }

        public override async Task<bool> TryEditTaskAsync(OneTimeTask oldTask, OneTimeTask newTask)
        {
            bool isValid = TaskValidator.ValidateTask(newTask);

            if (!isValid) return false;

            int index = Tasks.IndexOf(oldTask);

            if (index == -1) return false;

            newTask.ActionName = newTask.ActionName.Trim();

            Tasks[index] = newTask;
            if (ShouldSort) SortTasks();

            await _unitOfWork.OneTimeTaskRepository.SaveAsync(Tasks);
            return true;
        }

        public override async Task PerformTaskAsync(OneTimeTask task)
        {
            if (IsNullOrUnknownTask(task)) return;

            task.Perform();

            await _unitOfWork.OneTimeTaskRepository.SaveAsync(Tasks);
        }

        public override async Task ResetTaskAsync(OneTimeTask task)
        {
            if (IsNullOrUnknownTask(task)) return;

            task.Reset();

            await _unitOfWork.OneTimeTaskRepository.SaveAsync(Tasks);
        }

        public override async Task DeleteTaskAsync(OneTimeTask task)
        {
            if (IsNullOrUnknownTask(task, out int index)) return;

            Tasks.RemoveAt(index);

            await _unitOfWork.OneTimeTaskRepository.SaveAsync(Tasks);
        }

        protected override void SortTasks()
        {
            var sorted = Tasks.OrderBy(task => task.Priority)
                .ThenBy(task => task.ActionName.Length)
                .ToList();

            Tasks.Clear();

            foreach (OneTimeTask task in sorted)
            {
                Tasks.Add(task);
            }
        }
    }
}

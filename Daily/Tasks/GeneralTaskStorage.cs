using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Tasks
{
    public class GeneralTaskStorage : TaskStorage<GeneralTask>
    {
        public override ObservableCollection<GeneralTask>? Tasks { get; protected set; }

        public override int MaxTaskCount { get; } = 15;

        public GeneralTaskStorage(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public override async Task<ObservableCollection<GeneralTask>> LoadTasks()
        {
            IEnumerable<GeneralTask>? tasks = await _unitOfWork.GeneralTaskRepository.LoadAsync();

            Tasks = tasks == null ? new() : new(tasks);

            return Tasks;
        }

        public override async Task<bool> TryAddTaskAsync(GeneralTask task)
        {
            if (IsTasksFull) return false;

            bool isValid = TaskValidator.ValidateTask(task);
            bool contains = Tasks.Contains(task);

            if (!isValid || contains) return false;

            task.ActionName = task.ActionName.Trim();

            Tasks.Add(task);
            if (ShouldSort) SortTasks();

            await _unitOfWork.GeneralTaskRepository.SaveAsync(Tasks);

            return true;
        }

        public override async Task<bool> TryEditTaskAsync(GeneralTask oldTask, GeneralTask newTask)
        {
            bool isValid = TaskValidator.ValidateTask(newTask);

            if (!isValid) return false;

            int index = Tasks.IndexOf(oldTask);

            if (index == -1) return false;

            newTask.ActionName = newTask.ActionName.Trim();

            Tasks[index] = newTask;
            if (ShouldSort) SortTasks();

            await _unitOfWork.GeneralTaskRepository.SaveAsync(Tasks);
            return true;
        }

        public override async Task PerformTaskAsync(GeneralTask task)
        {
            if (IsNullOrUnknownTask(task)) return;

            task.Perform();

            await _unitOfWork.GeneralTaskRepository.SaveAsync(Tasks);
        }

        public override async Task ResetTaskAsync(GeneralTask task)
        {
            if (IsNullOrUnknownTask(task)) return;

            task.Reset();

            await _unitOfWork.GeneralTaskRepository.SaveAsync(Tasks);
        }

        public override async Task DeleteTaskAsync(GeneralTask task)
        {
            if (IsNullOrUnknownTask(task, out int index)) return;

            Tasks.RemoveAt(index);

            await _unitOfWork.GeneralTaskRepository.SaveAsync(Tasks);
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

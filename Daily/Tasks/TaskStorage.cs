using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Tasks
{
    public abstract class TaskStorage<T>
    {
        protected readonly IUnitOfWork _unitOfWork;

        public abstract ObservableCollection<T>? Tasks { get; protected set; }

        public abstract int MaxTaskCount { get; }

        public bool IsTasksFull => Tasks?.Count == MaxTaskCount;

        public bool ShouldSort => Tasks?.Count > 1;

        public TaskStorage(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public abstract Task LoadTasks();

        public abstract Task<bool> TryAddTaskAsync(T task);

        public abstract Task<bool> TryEditTaskAsync(T oldTask, T newTask);

        public abstract Task PerformTaskAsync(T task);

        public abstract Task ResetTaskAsync(T task);

        public abstract Task DeleteTaskAsync(T task);

        protected abstract void SortTasks();

        protected bool IsNullOrUnknownTask(T task)
        {
            return task == null || !Tasks.Contains(task);
        }

        protected bool IsNullOrUnknownTask(T task, out int index)
        {
            index = Tasks.IndexOf(task);

            return task == null || index == -1;
        }
    }
}

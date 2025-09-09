using Daily.Tasks;

namespace Daily.Data
{
    public interface IUnitOfWork
    {
        public IFileRepository<Goal> GoalRepository { get; }

        public IFileRepository<ICollection<OneTimeTask>> OneTimeTaskRepository { get; }

        public IFileRepository<ICollection<RecurringTask>> RecurringTaskRepository { get; }
    }
}
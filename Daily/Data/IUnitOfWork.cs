using Daily.Thoughts;
using Daily.Diary;
using Daily.Tasks;

namespace Daily.Data
{
    public interface IUnitOfWork : IDisposable
    {
        public IFileRepository<Goal> GoalRepository { get; }

        public IFileRepository<ICollection<GeneralTask>> GeneralTaskRepository { get; }

        public IFileRepository<ICollection<ConditionalTask>> ConditionalTaskRepository { get; }

        public IRepository<Thought> ThoughtRepository { get; }

        public IRepository<DiaryRecord> DiaryRecordRepository { get; }
    }
}
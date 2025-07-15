using Daily.Diary;
using Daily.Tasks;
using Daily.Thoughts;

namespace Daily.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SQLiteRecordsDatabaseConnectionProvider _connectionProvider = new();
        
        private IRepository<Thought>? _thoughtRepository;

        private IRepository<DiaryRecord>? _diaryRecordRepository;

        private IFileRepository<Goal>? _goalRepository;

        private IFileRepository<ICollection<GeneralTask>>? _generalTaskRepository;

        private IFileRepository<ICollection<ConditionalTask>>? _conditionalTaskRepository;

        public IFileRepository<Goal> GoalRepository
        {
            get => _goalRepository ??= new JsonFileRepository<Goal>(GoalFileName);
        }

        public IFileRepository<ICollection<GeneralTask>> GeneralTaskRepository
        {
            get => _generalTaskRepository ??= new JsonFileRepository<ICollection<GeneralTask>>(GeneralTasksFileName);
        }

        public IFileRepository<ICollection<ConditionalTask>> ConditionalTaskRepository
        {
            get => _conditionalTaskRepository ??= new JsonFileRepository<ICollection<ConditionalTask>>(ConditionalTasksFileName);
        }

        public IRepository<Thought> ThoughtRepository
        {
            get => _thoughtRepository ??= new SQLiteRepository<Thought>(_connectionProvider);
        }

        public IRepository<DiaryRecord> DiaryRecordRepository
        {
            get => _diaryRecordRepository ??= new SQLiteRepository<DiaryRecord>(_connectionProvider);
        }

        private const string GoalFileName = "goal.json";

        private const string GeneralTasksFileName = "generalTasks.json";
        private const string ConditionalTasksFileName = "conditionalTasks.json";

        public void Dispose()
        {
            _connectionProvider.Dispose();
        }
    }
}
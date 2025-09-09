using Daily.Diary;
using Daily.Tasks;

namespace Daily.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SQLiteRecordsDatabaseConnectionProvider _connectionProvider = new();
        
        private IRepository<DiaryRecord>? _diaryRecordRepository;

        private IFileRepository<Goal>? _goalRepository;

        private IFileRepository<ICollection<OneTimeTask>>? _oneTimeTaskRepository;

        private IFileRepository<ICollection<RecurringTask>>? _recurringTaskRepository;

        public IFileRepository<Goal> GoalRepository
        {
            get => _goalRepository ??= new JsonFileRepository<Goal>(GoalFileName);
        }

        public IFileRepository<ICollection<OneTimeTask>> OneTimeTaskRepository
        {
            get => _oneTimeTaskRepository ??= new JsonFileRepository<ICollection<OneTimeTask>>(OneTimeTasksFileName);
        }

        public IFileRepository<ICollection<RecurringTask>> RecurringTaskRepository
        {
            get => _recurringTaskRepository ??= new JsonFileRepository<ICollection<RecurringTask>>(RecurringTasksFileName);
        }

        public IRepository<DiaryRecord> DiaryRecordRepository
        {
            get => _diaryRecordRepository ??= new SQLiteRepository<DiaryRecord>(_connectionProvider);
        }

        private const string GoalFileName = "goal.json";

        private const string OneTimeTasksFileName = "generalTasks.json";
        private const string RecurringTasksFileName = "conditionalTasks.json";

        public void Dispose()
        {
            _connectionProvider.Dispose();
        }
    }
}
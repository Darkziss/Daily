using Daily.Diary;
using Daily.Thoughts;
using SQLite;

namespace Daily.Data
{
    public class SQLiteRecordsRepository : IRepository
    {
        private SQLiteAsyncConnection? _database;

        private readonly string _databasePath;

        private bool IsNotInitialized => _database == null;

        private bool IsInitialized => _database != null;

        private const string DatabaseFilename = "records.db3";

        private const SQLiteOpenFlags Flags = SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite;

        public SQLiteRecordsRepository(string folderPath)
        {
            _databasePath = Path.Combine(folderPath, DatabaseFilename);
        }

        public async Task InitAsync()
        {
            if (IsInitialized) return;

            _database = new(_databasePath, Flags);

            await _database.CreateTableAsync<DiaryRecord>();
            await _database.CreateTableAsync<Thought>();
        }

        public async Task<IReadOnlyList<DiaryRecord>> GetDiaryRecordsAsync()
        {
            if (_database == null)
                throw new InvalidOperationException(nameof(IsInitialized));
            
            return await _database.Table<DiaryRecord>().ToListAsync();
        }

        public async Task<IEnumerable<Thought>> GetThoughtsAsync()
        {
            if (_database == null)
                throw new InvalidOperationException(nameof(IsInitialized));

            return await _database.Table<Thought>().ToListAsync();
        }
    }
}
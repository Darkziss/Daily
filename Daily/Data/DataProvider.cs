using Daily.Tasks;
using Daily.Thoughts;
using Daily.Diary;
using AndroidApplication = Android.App.Application;
using AndroidEnvironment = Android.OS.Environment;

namespace Daily.Data
{
    public class DataProvider
    {
        private readonly string _goalDataPath;

        private readonly string _generalTasksDataPath;
        private readonly string _conditionalTasksDataPath;

        private readonly string _thoughtsDataPath;
        private readonly string _diaryRecordsDataPath;

        private readonly TextWriter _textWriter = new TextWriter();
        private readonly DataSerializer _dataSerializer = new JsonDataSerializer();

        private bool IsGoalFileExist => File.Exists(_goalDataPath);

        private bool IsGeneralTasksFileExist => File.Exists(_generalTasksDataPath);

        private bool IsThoughtsFileExist => File.Exists(_thoughtsDataPath);

        private bool IsDiaryRecordsFileExist => File.Exists(_diaryRecordsDataPath);

        private const string goalDataFileName = "goal.json";

        private const string generalTasksDataFileName = "generalTasks.json";
        private const string conditionalTasksDataFileName = "conditionalTasks.json";

        private const string thoughtsDataFileName = "thoughts.json";
        private const string diaryRecordsDataFileName = "diaryRecords.json";

        public DataProvider()
        {
            string dataFolderPath = AndroidApplication.Context.
                GetExternalFilesDir(AndroidEnvironment.DirectoryDocuments)!.AbsolutePath;

            _goalDataPath = Path.Combine(dataFolderPath, goalDataFileName);

            _generalTasksDataPath = Path.Combine(dataFolderPath, generalTasksDataFileName);
            _conditionalTasksDataPath = Path.Combine(dataFolderPath, conditionalTasksDataFileName);

            _thoughtsDataPath = Path.Combine(dataFolderPath, thoughtsDataFileName);
            _diaryRecordsDataPath = Path.Combine(dataFolderPath, diaryRecordsDataFileName);
        }
        
        public async Task SaveGoalAsync(Goal goal)
        {
            await _dataSerializer.SerializeAsync(_goalDataPath, goal);
        }

        public async Task SaveGeneralTasksAsync(IReadOnlyList<GeneralTask> generalTasks)
        {
            await _dataSerializer.SerializeAsync<IReadOnlyList<GeneralTask>>(_generalTasksDataPath, generalTasks);
        }

        public async Task SaveConditionalTasksAsync(IReadOnlyList<ConditionalTask> сonditionalTasks)
        {
            await _dataSerializer.SerializeAsync<IReadOnlyList<ConditionalTask>>(_conditionalTasksDataPath, сonditionalTasks);
        }

        public async Task SaveThoughtsAsync(IReadOnlyList<Thought> thoughts)
        {
            await _dataSerializer.SerializeAsync<IReadOnlyList<Thought>>(_thoughtsDataPath, thoughts);
        }

        public async Task SaveDiaryRecordsAsync(IReadOnlyList<DiaryRecord> diaryRecords)
        {
            await _dataSerializer.SerializeAsync<IReadOnlyList<DiaryRecord>>(_diaryRecordsDataPath, diaryRecords);
        }

        [Obsolete]
        private Goal? LoadGoal()
        {
            bool exists = File.Exists(_goalDataPath);

            if (exists) return _dataSerializer.Deserialize<Goal>(_goalDataPath);
            else return null;
        }

        public async Task<Goal?> LoadGoalAsync()
        {
            if (IsGoalFileExist)
                return await _dataSerializer.DeserializeAsync<Goal>(_goalDataPath);
            else
                return null;
        }

        private IReadOnlyList<GeneralTask>? LoadGeneralTasks()
        {
            bool exists = File.Exists(_generalTasksDataPath);

            if (exists)
            {
                return _dataSerializer.Deserialize<IReadOnlyList<GeneralTask>>(_generalTasksDataPath);
            }
            else return null;
        }

        public async Task<IEnumerable<GeneralTask>?> LoadGeneralTasksAsync()
        {
            if (IsGeneralTasksFileExist)
                return await _dataSerializer.DeserializeAsync<IEnumerable<GeneralTask>>(_generalTasksDataPath);
            else 
                return null;
        }

        public async Task<IEnumerable<ConditionalTask>?> LoadConditionalTasksAsync()
        {
            bool exists = File.Exists(_conditionalTasksDataPath);

            if (exists)
                return await _dataSerializer.DeserializeAsync<IEnumerable<ConditionalTask>>(_conditionalTasksDataPath);
            else
                return null;
        }

        [Obsolete]
        private IReadOnlyList<Thought>? LoadThoughts()
        {
            bool exists = File.Exists(_thoughtsDataPath);

            if (exists)
            {
                return _dataSerializer.Deserialize<IReadOnlyList<Thought>>(_thoughtsDataPath);
            }
            else return null;
        }

        public async Task<IEnumerable<Thought>?> LoadThoughtsAsync()
        {
            if (IsThoughtsFileExist)
                return await _dataSerializer.DeserializeAsync<IEnumerable<Thought>>(_thoughtsDataPath);
            else
                return null;
        }

        [Obsolete]
        private IReadOnlyList<DiaryRecord>? LoadDiaryRecords()
        {
            bool exists = File.Exists(_diaryRecordsDataPath);

            if (exists)
            {
                return _dataSerializer.Deserialize<IReadOnlyList<DiaryRecord>>(_diaryRecordsDataPath);
            }
            else return null;
        }

        public async Task<IEnumerable<DiaryRecord>?> LoadDiaryRecordsAsync()
        {
            if (IsDiaryRecordsFileExist)
                return await _dataSerializer.DeserializeAsync<IEnumerable<DiaryRecord>>(_diaryRecordsDataPath);
            else
                return null;
        }
    }
}
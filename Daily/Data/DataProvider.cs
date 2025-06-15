using Daily.Tasks;
using Daily.Thoughts;
using Daily.Diary;
using AndroidApplication = Android.App.Application;
using AndroidEnvironment = Android.OS.Environment;

namespace Daily.Data
{
    public class DataProvider
    {
        public Goal? Goal { get; private set; }

        public IReadOnlyList<GeneralTask>? GeneralTasks { get; private set; }
        public IReadOnlyList<ConditionalTask>? СonditionalTasks { get; private set; }

        public IReadOnlyList<Thought>? Thoughts { get; private set; }
        public IReadOnlyList<DiaryRecord>? DiaryRecords { get; private set; }

        private readonly string _goalDataPath;

        private readonly string _generalTasksDataPath;
        private readonly string _conditionalTasksDataPath;

        private readonly string _thoughtsDataPath;
        private readonly string _diaryRecordsDataPath;

        private readonly TextWriter _textWriter = new TextWriter();
        private readonly DataSerializer _dataSerializer = new JsonDataSerializer();

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

            Goal = LoadGoal();
            
            GeneralTasks = LoadGeneralTasks();
            СonditionalTasks = LoadСonditionalTasks();

            Thoughts = LoadThoughts();
            DiaryRecords = LoadDiaryRecords();
        }
        
        public async Task SaveGoalAsync(Goal goal)
        {
            Goal = goal;

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

        private Goal? LoadGoal()
        {
            bool exists = File.Exists(_goalDataPath);

            if (exists) return _dataSerializer.Deserialize<Goal>(_goalDataPath);
            else return null;
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

        private IReadOnlyList<ConditionalTask>? LoadСonditionalTasks()
        {
            bool exists = File.Exists(_conditionalTasksDataPath);

            if (exists)
            {
                return _dataSerializer.Deserialize<IReadOnlyList<ConditionalTask>>(_conditionalTasksDataPath);
            }
            else return null;
        }

        private IReadOnlyList<Thought>? LoadThoughts()
        {
            bool exists = File.Exists(_thoughtsDataPath);

            if (exists)
            {
                return _dataSerializer.Deserialize<IReadOnlyList<Thought>>(_thoughtsDataPath);
            }
            else return null;
        }

        private IReadOnlyList<DiaryRecord>? LoadDiaryRecords()
        {
            bool exists = File.Exists(_diaryRecordsDataPath);

            if (exists)
            {
                return _dataSerializer.Deserialize<IReadOnlyList<DiaryRecord>>(_diaryRecordsDataPath);
            }
            else return null;
        }
    }
}
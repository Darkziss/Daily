using System.Text.Json;
using AndroidFile = Java.IO.File;
using AndroidApplication = Android.App.Application;
using AndroidEnvironment = Android.OS.Environment;
using Daily.Tasks;

namespace Daily.Data
{
    public class DataProvider
    {
        public string? Goal { get; private set; }

        public IReadOnlyList<GeneralTask>? GeneralTasks { get; private set; }
        public IReadOnlyList<СonditionalTask>? СonditionalTasks { get; private set; }

        private readonly string _goalDataPath;

        private readonly string _generalTasksDataPath;
        private readonly string _conditionalTasksDataPath;

        private readonly TextWriter _textWriter = new TextWriter();
        private readonly DataSerializer _dataSerializer = new JsonDataSerializer();

        private const string goalDataFileName = "goal.txt";

        private const string generalTasksDataFileName = "generalTasks.json";
        private const string conditionalTasksDataFileName = "conditionalTasks.json";

        public DataProvider()
        {
            string dataFolderPath = AndroidApplication.Context.
                GetExternalFilesDir(AndroidEnvironment.DirectoryDocuments)!.AbsolutePath;

            _goalDataPath = Path.Combine(dataFolderPath, goalDataFileName);

            _generalTasksDataPath = Path.Combine(dataFolderPath, generalTasksDataFileName);
            _conditionalTasksDataPath = Path.Combine(dataFolderPath, conditionalTasksDataFileName);

            Goal = LoadGoal();
            
            GeneralTasks = LoadGeneralTasks();
            СonditionalTasks = LoadСonditionalTasks();
        }
        
        public async Task SaveGoalAsync(string goal)
        {
            Goal = goal;

            await _textWriter.WriteTextAsync(_goalDataPath, goal);
        }

        public async Task SaveGeneralTasksAsync(IReadOnlyList<GeneralTask> generalTasks)
        {
            await _dataSerializer.SerializeAsync<IReadOnlyList<GeneralTask>>(_generalTasksDataPath, generalTasks);
        }

        public async Task SaveConditionalTasksAsync(IReadOnlyList<СonditionalTask> сonditionalTasks)
        {
            await _dataSerializer.SerializeAsync<IReadOnlyList<СonditionalTask>>(_conditionalTasksDataPath, сonditionalTasks);
        }

        private string? LoadGoal()
        {
            bool exists = File.Exists(_goalDataPath);

            if (exists) return _textWriter.ReadText(_goalDataPath);
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

        private IReadOnlyList<СonditionalTask>? LoadСonditionalTasks()
        {
            bool exists = File.Exists(_conditionalTasksDataPath);

            if (exists)
            {
                return _dataSerializer.Deserialize<IReadOnlyList<СonditionalTask>>(_conditionalTasksDataPath);
            }
            else return null;
        }
    }
}
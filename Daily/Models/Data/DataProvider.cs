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
        public List<GeneralTask>? GeneralTasks { get; private set; }

        private readonly string _goalPath;
        private readonly string _generalTasksDataPath;

        private readonly TextWriter _textWriter = new TextWriter();
        private readonly DataSerializer _dataSerializer = new JsonDataSerializer();

        private const string goalDataFileName = "goal.txt";
        private const string generalTasksDataFileName = "generalTasks.json";

        private const FileMode saveFileMode = FileMode.Create;
        private const FileMode loadFileMode = FileMode.Open;

        public DataProvider()
        {
            AndroidFile dataFolder = AndroidApplication.Context.
                GetExternalFilesDir(AndroidEnvironment.DirectoryDocuments)!;

            _goalPath = Path.Combine(dataFolder.AbsolutePath, goalDataFileName);
            _generalTasksDataPath = Path.Combine(dataFolder.AbsolutePath, generalTasksDataFileName);

            Goal = LoadGoal();
            GeneralTasks = LoadGeneralTasks();
        }
        
        public async Task SaveGoalAsync(string goal)
        {
            Goal = goal;

            await _textWriter.WriteTextAsync(_goalPath, goal);
        }

        public async Task SaveGeneralTasksAsync(List<GeneralTask> generalTasks)
        {
            await _dataSerializer.SerializeAsync<List<GeneralTask>>(_generalTasksDataPath, generalTasks);
        }

        private string? LoadGoal()
        {
            bool exists = File.Exists(_goalPath);

            if (exists) return _textWriter.ReadText(_goalPath);
            else return null;
        }

        private List<GeneralTask>? LoadGeneralTasks()
        {
            bool exists = File.Exists(_generalTasksDataPath);

            if (exists) return _dataSerializer.Deserialize<List<GeneralTask>>(_generalTasksDataPath);
            else return null;
        }
    }
}
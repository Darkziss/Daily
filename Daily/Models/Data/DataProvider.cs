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
        public ICollection<GeneralTask>? GeneralTasks { get; private set; }

        private readonly string _goalDataPath;
        private readonly string _generalTasksDataPath;

        private readonly TextWriter _textWriter = new TextWriter();
        private readonly DataSerializer _dataSerializer = new JsonDataSerializer();

        private const string goalDataFileName = "goal.txt";
        private const string generalTasksDataFileName = "generalTasks.json";

        public DataProvider()
        {
            string dataFolderPath = AndroidApplication.Context.
                GetExternalFilesDir(AndroidEnvironment.DirectoryDocuments)!.AbsolutePath;

            _goalDataPath = Path.Combine(dataFolderPath, goalDataFileName);
            _generalTasksDataPath = Path.Combine(dataFolderPath, generalTasksDataFileName);

            Goal = LoadGoal();
            GeneralTasks = LoadGeneralTasks();
        }
        
        public async Task SaveGoalAsync(string goal)
        {
            Goal = goal;

            await _textWriter.WriteTextAsync(_goalDataPath, goal);
        }

        public async Task SaveGeneralTasksAsync(ICollection<GeneralTask> generalTasks)
        {
            await _dataSerializer.SerializeAsync<ICollection<GeneralTask>>(_generalTasksDataPath, generalTasks);
        }

        private string? LoadGoal()
        {
            bool exists = File.Exists(_goalDataPath);

            if (exists) return _textWriter.ReadText(_goalDataPath);
            else return null;
        }

        private ICollection<GeneralTask>? LoadGeneralTasks()
        {
            bool exists = File.Exists(_generalTasksDataPath);

            if (exists) return _dataSerializer.Deserialize<ICollection<GeneralTask>>(_generalTasksDataPath);
            else return null;
        }
    }
}
using System.Text.Json;
using AndroidFile = Java.IO.File;
using AndroidApplication = Android.App.Application;
using AndroidEnvironment = Android.OS.Environment;
using Daily.Tasks;
using Android.AdServices.Common;

namespace Daily.Data
{
    public class DataProvider
    {
        private readonly AppData _appData;
        private List<GeneralTask> _generalTasks;

        private readonly string _appDataPath;
        private readonly string _generalTasksDataPath;

        private const string appDataFileName = "appData.json";
        private const string generalTasksDataFileName = "generalTasks.json";

        private const FileMode saveFileMode = FileMode.Create;
        private const FileMode loadFileMode = FileMode.Open;

        public DataProvider()
        {
            AndroidFile dataFolder = AndroidApplication.Context.
                GetExternalFilesDir(AndroidEnvironment.DirectoryDocuments)!;

            _appDataPath = Path.Combine(dataFolder.AbsolutePath, appDataFileName);
            _generalTasksDataPath = Path.Combine(dataFolder.AbsolutePath, generalTasksDataFileName);

            _appData = LoadAppData();
            _generalTasks = LoadGeneralTasks();
        }
        
        public async Task SaveGoalAsync(string goal)
        {
            _appData.goal = goal;

            await SerializeAppDataAsync();
        }

        public string LoadGoal() => _appData.goal;

        public async Task SaveGeneralTasks(List<GeneralTask> generalTasks)
        {
            _generalTasks = generalTasks;

            await SerializeGeneralTasksAsync();
        }

        public List<GeneralTask> GetGeneralTasks() => _generalTasks;

        private AppData LoadAppData()
        {
            bool exists = File.Exists(_appDataPath);

            if (exists) return DeserializeAppData();
            else return CreateAndSerializeAppData();
        }

        private List<GeneralTask> LoadGeneralTasks()
        {
            bool exits = File.Exists(_generalTasksDataPath);

            if (exits) return DeserializeGeneralTasks();
            else return CreateAndSerializeGeneralTasks();
        }

        private AppData CreateAndSerializeAppData()
        {
            AppData appData = new AppData();
            
            using (FileStream stream = File.Create(_appDataPath))
            {
                JsonSerializer.Serialize<AppData>(stream, appData);
            }

            return appData;
        }

        private async Task SerializeAppDataAsync()
        {
            using (FileStream stream = new FileStream(_appDataPath, saveFileMode))
            {
                await JsonSerializer.SerializeAsync<AppData>(stream, _appData);
            }
        }

        private AppData DeserializeAppData()
        {
            using (FileStream stream = new FileStream(_appDataPath, loadFileMode))
            {
                AppData? data = JsonSerializer.Deserialize<AppData>(stream);

                return data!;
            }
        }

        private List<GeneralTask> CreateAndSerializeGeneralTasks()
        {
            List<GeneralTask> tasks = new List<GeneralTask>(0);

            using (FileStream stream = File.Create(_generalTasksDataPath))
            {
                JsonSerializer.Serialize<List<GeneralTask>>(stream, tasks);
            }

            return tasks;
        }

        private async Task SerializeGeneralTasksAsync()
        {
            using (FileStream stream = new FileStream(_generalTasksDataPath, saveFileMode))
            {
                await JsonSerializer.SerializeAsync<List<GeneralTask>>(stream, _generalTasks);
            }
        }

        private List<GeneralTask> DeserializeGeneralTasks()
        {
            using (FileStream stream = new FileStream(_generalTasksDataPath, loadFileMode))
            {
                return JsonSerializer.Deserialize<List<GeneralTask>>(stream)!;
            }
        }
    }
}
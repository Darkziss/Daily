using System.Text.Json;
using AndroidFile = Java.IO.File;
using AndroidApplication = Android.App.Application;
using AndroidEnvironment = Android.OS.Environment;

namespace Daily.Data
{
    public class DataProvider
    {
        private readonly AppData _appData;

        private readonly string _appDataPath;

        private const string fileName = "appData.json";

        private const FileMode saveFileMode = FileMode.Create;
        private const FileMode loadFileMode = FileMode.Open;

        public DataProvider()
        {
            AndroidFile dataFolder = AndroidApplication.Context.
                GetExternalFilesDir(AndroidEnvironment.DirectoryDocuments)!;

            _appDataPath = Path.Combine(dataFolder.AbsolutePath, fileName);

            _appData = LoadAppData();
        }
        
        public async Task SaveGoalAsync(string goal)
        {
            _appData.goal = goal;

            await SerializeAppDataAsync();
        }

        public string LoadGoal() => _appData.goal;

        private AppData LoadAppData()
        {
            bool exists = File.Exists(_appDataPath);

            if (exists) return DeserializeAppData();
            else return CreateAndSerializeAppData();
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
    }
}
using System.Text.Json;
using System.Diagnostics;
using AndroidFile = Java.IO.File;
using AndroidApplication = Android.App.Application;
using AndroidEnvironment = Android.OS.Environment;

namespace Daily.Data
{
    public class DataProvider
    {
        private readonly AppData _appData;

        private readonly string _savePath;

        private const string fileName = "appData.json";

        private const FileMode fileMode = FileMode.OpenOrCreate;

        public DataProvider()
        {
            AndroidFile? documentsFolderFile = AndroidApplication.Context.GetExternalFilesDir(AndroidEnvironment.DirectoryDocuments);
            string appDataPath = documentsFolderFile!.AbsoluteFile.Path;

            _savePath = $"{appDataPath}/{fileName}";

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
            bool exists = File.Exists(_savePath);
            AppData? appData;

            if (exists)
            {
                appData = DeserializeAppData();

                return appData!;
            }
            else
            {
                appData = new AppData();

                using (FileStream stream = File.Create(_savePath))
                {
                    JsonSerializer.Serialize<AppData>(stream, appData);
                }

                return appData;
            }
        }

        private async Task SerializeAppDataAsync()
        {
            using (FileStream stream = new FileStream(_savePath, fileMode))
            {
                await JsonSerializer.SerializeAsync<AppData>(stream, _appData);
            }
        }

        private AppData? DeserializeAppData()
        {
            using (FileStream stream = new FileStream(_savePath, fileMode))
            {
                AppData? data = JsonSerializer.Deserialize<AppData>(stream);

                return data;
            }
        }
    }
}
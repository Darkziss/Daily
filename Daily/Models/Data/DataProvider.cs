using System.Text.Json;

namespace Daily.Data
{
    public class DataProvider
    {
        private readonly AppData _appData;

        private readonly string _savePath = $@"{FileSystem.AppDataDirectory}/{fileName}";

        private const string fileName = "appData.json";

        private const FileMode fileMode = FileMode.OpenOrCreate;

        public DataProvider()
        {
            AppData? data = LoadAppData();

            if (data != null) _appData = data;
            else _appData = new AppData();
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

                return appData == null ? new AppData() : appData;
            }
            else
            {
                using (FileStream stream = File.Create(_savePath))
                {
                    appData = new AppData();

                    JsonSerializer.Serialize<AppData>(appData);
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
using System.Text.Json.Serialization;

namespace Daily.Data
{
    public class AppData
    {
        [JsonInclude] public string goal;

        public AppData()
        {
            goal = string.Empty;
        }
    }
}
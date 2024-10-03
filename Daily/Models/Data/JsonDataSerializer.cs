using System.Text.Json;

namespace Daily.Data
{
    public class JsonDataSerializer : DataSerializer
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions() { WriteIndented = true };

        public override void Serialize<T>(string path, T value)
        {
            using (FileStream stream = new FileStream(path, writeFileMode))
            {
                JsonSerializer.Serialize(stream, value, _options);
            }
        }

        public override T Deserialize<T>(string path)
        {
            using (FileStream stream = new FileStream(path, readFileMode))
            {
                return JsonSerializer.Deserialize<T>(stream)!;
            }
        }

        public override async Task SerializeAsync<T>(string path, T value)
        {
            using (FileStream stream = new FileStream(path, writeFileMode))
            {
                await JsonSerializer.SerializeAsync(stream, value);
            }
        }
    }
}
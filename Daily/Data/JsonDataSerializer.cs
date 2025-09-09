using System.Text.Json;
using System.Text.Unicode;
using System.Text.Encodings.Web;

namespace Daily.Data
{
    public class JsonDataSerializer : DataSerializer
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            IncludeFields = false,
            IgnoreReadOnlyProperties = true,
#if DEBUG
            WriteIndented = true,
#else
            WriteIndented = false,
#endif
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
        };

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
                await JsonSerializer.SerializeAsync(stream, value, _options);
            }
        }

        public override async Task<T?> DeserializeAsync<T>(string path) where T : default
        {
            using (FileStream stream = new(path, readFileMode))
            {
                return await JsonSerializer.DeserializeAsync<T>(stream, _options);
            }
        }
    }
}
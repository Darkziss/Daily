using Daily.Tasks;

namespace Daily.Data
{
    public class JsonFileRepository<T> : IFileRepository<T> where T : class
    {
        private readonly JsonDataSerializer _serializer;

        private readonly string _path;

        public JsonFileRepository(string fileName)
        {
            _serializer = new();

            _path = DataFolderPath.CombineWith(fileName);
        }

        public async Task SaveAsync(T item)
        {
            await _serializer.SerializeAsync(_path, item);
        }

        public async Task<T?> LoadAsync()
        {
            if (!File.Exists(_path))
                return null;
            else
                return await _serializer.DeserializeAsync<T>(_path);
        }
    }
}
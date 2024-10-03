
namespace Daily.Data
{
    public abstract class DataSerializer
    {
        protected const FileMode writeFileMode = FileMode.Create;
        protected const FileMode readFileMode = FileMode.Open;

        public abstract void Serialize<T>(string path, T value);

        public abstract T Deserialize<T>(string path);

        public abstract Task SerializeAsync<T>(string path, T value);
    }
}

namespace Daily.Data
{
    public interface IFileRepository<T> where T : class
    {
        public Task SaveAsync(T item);

        public Task<T?> LoadAsync();
    }
}
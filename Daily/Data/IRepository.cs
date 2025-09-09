using Daily.Diary;

namespace Daily.Data
{
    public interface IRepository<T> where T : class, new()
    {
        public void Insert(T item);

        public void Update(T item);

        public void Delete(T item);

        public IEnumerable<T> GetAll();
    }
}

namespace Daily.Data
{
    public class SQLiteRepository<T> : IRepository<T> where T : class, new()
    {
        private readonly SQLiteRecordsDatabaseConnectionProvider _connectionProvider;

        public SQLiteRepository(SQLiteRecordsDatabaseConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;

            _connectionProvider.Connection.CreateTable<T>();
        }

        public void Insert(T item)
        {
            _connectionProvider.Connection.Insert(item);
        }

        public void Update(T item)
        {
            _connectionProvider.Connection.Update(item);
        }

        public void Delete(T item)
        {
            _connectionProvider.Connection.Delete(item);
        }

        public IEnumerable<T> GetAll()
        {
            return _connectionProvider.Connection.Table<T>().ToList();
        }
    }
}
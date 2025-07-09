using SQLite;

namespace Daily.Data
{
    public class SQLiteRecordsDatabaseConnectionProvider : IDisposable
    {
        public SQLiteConnection Connection { get; }

        private const string DataBaseName = "records.db3";

        private const SQLiteOpenFlags Flags = SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite;

        public SQLiteRecordsDatabaseConnectionProvider()
        {
            string path = DataFolderPath.CombineWith(DataBaseName);

            Connection = new(path);
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
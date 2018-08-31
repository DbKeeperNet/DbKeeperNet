using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.SQLite
{
    public class SQLiteDatabaseService : IDatabaseService, IDatabaseService<SqliteConnection>
    {
        private readonly SqliteConnection _connection;

        public SQLiteDatabaseService(string connectionString)
        {
            _connection = new SqliteConnection(connectionString);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        public bool CanHandle(string databaseType)
        {
            var status = false;

            if (databaseType != null)
            {
                switch (databaseType.ToUpperInvariant())
                {
                    case @"SQLITE":
                        status = true;
                        break;
                }
            }

            return status;
        }

        public DbConnection GetOpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            return _connection;
        }

        public DbConnection CreateOpenConnection()
        {
            var connection = new SqliteConnection(_connection.ConnectionString);
            connection.Open();
            return connection;
        }

        SqliteConnection IDatabaseService<SqliteConnection>.GetOpenConnection()
        {
            return (SqliteConnection) GetOpenConnection();
        }
    }
}
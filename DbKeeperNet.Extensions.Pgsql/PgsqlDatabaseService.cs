using System.Data;
using System.Data.Common;
using DbKeeperNet.Engine;
using Npgsql;

namespace DbKeeperNet.Extensions.Pgsql
{
    public class PgsqlDatabaseService : IDatabaseService, IDatabaseService<NpgsqlConnection>
    {
        private readonly NpgsqlConnection _connection;

        public PgsqlDatabaseService(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
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
                    case @"PGSQL":
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
            var connection = new NpgsqlConnection(_connection.ConnectionString);
            connection.Open();
            return connection;
        }

        NpgsqlConnection IDatabaseService<NpgsqlConnection>.GetOpenConnection()
        {
            return (NpgsqlConnection) GetOpenConnection();
        }
    }
}
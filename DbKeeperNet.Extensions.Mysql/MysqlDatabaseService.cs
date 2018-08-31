using System.Data;
using System.Data.Common;
using DbKeeperNet.Engine;
using MySql.Data.MySqlClient;

namespace DbKeeperNet.Extensions.Mysql
{
    public class MysqlDatabaseService : IDatabaseService, IDatabaseService<MySqlConnection>
    {
        private readonly MySqlConnection _connection;

        public MysqlDatabaseService(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
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
                    case @"MYSQL":
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
            var connection = new MySqlConnection(_connection.ConnectionString);
            connection.Open();
            return connection;
        }
        
        MySqlConnection IDatabaseService<MySqlConnection>.GetOpenConnection()
        {
            return (MySqlConnection) GetOpenConnection();
        }
    }
}
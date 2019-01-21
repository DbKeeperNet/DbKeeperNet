using System.Data;
using System.Data.Common;
using DbKeeperNet.Engine;
using FirebirdSql.Data.FirebirdClient;

namespace DbKeeperNet.Extensions.Firebird
{
    public class FirebirdDatabaseService : IDatabaseService, IDatabaseService<FbConnection>
    {
        private readonly FbConnection _connection;

        public FirebirdDatabaseService(IConnectionStringProvider connectionStringProvider)
        {
            _connection = new FbConnection(connectionStringProvider.ConnectionString);
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
                    case @"FB":
                    case @"FIREBIRD":
                        status = true;
                        break;
                }
            }

            return status;
        }

        public DbConnection CreateOpenConnection()
        {
            var connection = new FbConnection(_connection.ConnectionString);
            connection.Open();
            return connection;
        }

        public DbConnection GetOpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            return _connection;
        }

        FbConnection IDatabaseService<FbConnection>.GetOpenConnection()
        {
            return (FbConnection) GetOpenConnection();
        }
    }
}
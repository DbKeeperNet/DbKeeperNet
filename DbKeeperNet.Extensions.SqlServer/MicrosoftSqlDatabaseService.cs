using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DbKeeperNet.Engine;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Extensions.SqlServer
{
    public class MicrosoftSqlDatabaseService : IDatabaseService, IDatabaseService<SqlConnection>
    {
        private readonly ILogger<MicrosoftSqlDatabaseService> _logger;
        private readonly SqlConnection _connection;

        public MicrosoftSqlDatabaseService(string connectionString, ILogger<MicrosoftSqlDatabaseService> logger)
        {
            _logger = logger;

            _connection = new SqlConnection();
            _connection.ConnectionString = connectionString;
        }

        public bool CanHandle(string databaseType)
        {
            bool status = false;

            if (databaseType != null)
            {
                switch (databaseType.ToUpperInvariant())
                {
                    case @"MSSQL":
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
                _logger.LogInformation("Database connection is {0} - going to open it", _connection.State);
                _connection.Open();
            }

            _logger.LogTrace("Going to use already open connection", _connection.State);

            return _connection;
        }

        public DbConnection CreateOpenConnection()
        {
            var connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            return connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        SqlConnection IDatabaseService<SqlConnection>.GetOpenConnection()
        {
            return (SqlConnection) GetOpenConnection();
        }
    }
}
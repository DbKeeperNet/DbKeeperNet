using System.Data;
using System.Data.SqlClient;
using DbKeeperNet.Engine;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Extensions.SqlServer
{
    public class MicrosoftSqlDatabaseLock : IDatabaseLock
    {
        private readonly ILogger<MicrosoftSqlDatabaseLock> _logger;
        private readonly IDatabaseServiceStoredProcedureChecker _checker;
        private readonly IDatabaseService<SqlConnection> _databaseService;

        public MicrosoftSqlDatabaseLock(ILogger<MicrosoftSqlDatabaseLock> logger, IDatabaseServiceStoredProcedureChecker checker, IDatabaseService<SqlConnection> databaseService)
        {
            _logger = logger;
            _checker = checker;
            _databaseService = databaseService;
        }

        public bool IsSupported
        {
            get { return _checker.Exists("DbKeeperNetAcquireLock"); }
        }

        public bool Acquire(int lockId, string ownerDescription, int expirationMinutes)
        {
            using (var command = new SqlCommand("DbKeeperNetAcquireLock", _databaseService.GetOpenConnection()))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.Int).Value = lockId;
                command.Parameters.Add("@ownerDescription", SqlDbType.NVarChar).Value = ownerDescription;
                command.Parameters.Add("@expiration", SqlDbType.Int).Value = expirationMinutes;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _logger.LogInformation("Database lock {0} {1} is owned by {2} and expires {3}", reader["id"], reader["description"], reader["ownerDescription"], reader["expiration"] );
                        return false;
                    }
                    
                    return true;
                }
            }
        }

        public void Release(int lockId)
        {
            using (var command = new SqlCommand("DbKeeperNetReleaseLock", _databaseService.GetOpenConnection()))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.Int).Value = lockId;
                command.ExecuteNonQuery();
            }
        }
    }
}
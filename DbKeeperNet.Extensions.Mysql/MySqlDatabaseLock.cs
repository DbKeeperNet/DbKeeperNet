using System.Data;
using DbKeeperNet.Engine;
using MySql.Data.MySqlClient;

namespace DbKeeperNet.Extensions.Mysql
{
    public class MySqlDatabaseLock : IDatabaseLock
    {
        private readonly IDatabaseServiceTableChecker _tableChecker;
        private readonly IDatabaseService<MySqlConnection> _databaseService;

        public MySqlDatabaseLock(IDatabaseServiceTableChecker tableChecker, IDatabaseService<MySqlConnection> databaseService)
        {
            _tableChecker = tableChecker;
            _databaseService = databaseService;
        }

        public bool IsSupported
        {
            get
            {
                // this check happens possible even before the initial tables are
                // created so cannot use check whether particular upgrade was already done
                // regardless - lock should be acquired only once
                return _tableChecker.Exists("dbkeepernet_lock");
            }
        }

        public bool Acquire(int lockId, string ownerDescription, int expirationMinutes)
        {
            var connection = _databaseService.GetOpenConnection();
            using (var transaction = connection.BeginTransaction())
            using (var cmd = new MySqlCommand($"UPDATE dbkeepernet_lock SET expiration = DATE_ADD(UTC_TIMESTAMP(), INTERVAL {expirationMinutes} MINUTE) WHERE id = @id AND expiration < UTC_TIMESTAMP()", connection))
            {
                var id = new MySqlParameter("@id", DbType.Int32) { Value = lockId };
                cmd.Parameters.Add(id);
                cmd.Transaction = transaction;
                var result = cmd.ExecuteNonQuery() == 0;
                transaction.Commit();

                if (result)
                {
                    return false;
                }

                return true;
            }
        }

        public void Release(int lockId)
        {
            var connection = _databaseService.GetOpenConnection();
            using (var transaction = connection.BeginTransaction())
            using (var cmd = new MySqlCommand(@"UPDATE dbkeepernet_lock SET expiration = DATE_ADD(UTC_TIMESTAMP(), INTERVAL -1 MINUTE) WHERE id = @id", connection))
            {
                var id = new MySqlParameter("@id", DbType.Int32) { Value = lockId };
                cmd.Parameters.Add(id);
                cmd.Transaction = transaction;
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }
    }
}
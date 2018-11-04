using System.Data;
using DbKeeperNet.Engine;
using FirebirdSql.Data.FirebirdClient;

namespace DbKeeperNet.Extensions.Firebird
{
    public class FirebirdDatabaseLock : IDatabaseLock
    {
        private readonly IDatabaseServiceTableChecker _tableChecker;
        private readonly IDatabaseService<FbConnection> _databaseService;

        public FirebirdDatabaseLock(IDatabaseServiceTableChecker tableChecker, IDatabaseService<FbConnection> databaseService)
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
            using (var cmd = new FbCommand($"UPDATE \"dbkeepernet_lock\" SET \"expiration\" = DATEADD(minute, {expirationMinutes}, CURRENT_TIMESTAMP) WHERE \"id\" = @id AND \"expiration\" < CURRENT_TIMESTAMP", connection))
            {
                var id = new FbParameter("@id", DbType.Int32) { Value = lockId };
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
            using (var cmd = new FbCommand(@"UPDATE ""dbkeepernet_lock"" SET ""expiration"" = DATEADD(minute, -1, CURRENT_TIMESTAMP) WHERE ""id"" = @id", connection))
            {
                var id = new FbParameter("@id", DbType.Int32) { Value = lockId };
                cmd.Parameters.Add(id);
                cmd.Transaction = transaction;
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }
    }
}
using DbKeeperNet.Engine;
using Microsoft.Data.Sqlite;

namespace DbKeeperNet.Extensions.SQLite
{
    public class SQLiteDatabaseLock : IDatabaseLock
    {
        private readonly IDatabaseServiceTableChecker _tableChecker;
        private readonly IDatabaseService<SqliteConnection> _databaseService;

        public SQLiteDatabaseLock(IDatabaseServiceTableChecker tableChecker, IDatabaseService<SqliteConnection> databaseService)
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
            using (var cmd = new SqliteCommand($"UPDATE dbkeepernet_lock SET expiration = datetime('now', 'utc', '{expirationMinutes} minutes') WHERE id = @id AND expiration < datetime('now', 'utc')", connection))
            {
                var id = new SqliteParameter("@id", SqliteType.Integer) { Value = lockId };
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
            using (var cmd = new SqliteCommand(@"UPDATE dbkeepernet_lock SET expiration = datetime('now', 'utc', '-1 minutes') WHERE id = @id", connection))
            {
                var id = new SqliteParameter("@id", SqliteType.Integer) {Value = lockId};
                cmd.Parameters.Add(id);
                cmd.Transaction = transaction;

                cmd.ExecuteNonQuery();

                transaction.Commit();
            }
        }
    }
}
using System.Data;
using DbKeeperNet.Engine;
using Npgsql;

namespace DbKeeperNet.Extensions.Pgsql
{
    public class PgSqlDatabaseLock : IDatabaseLock
    {
        private readonly IDatabaseServiceTableChecker _tableChecker;
        private readonly IDatabaseService<NpgsqlConnection> _databaseService;

        public PgSqlDatabaseLock(IDatabaseServiceTableChecker tableChecker, IDatabaseService<NpgsqlConnection> databaseService)
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
            var npgsqlConnection = _databaseService.GetOpenConnection();
            using (var transaction = npgsqlConnection.BeginTransaction())
            using (var cmd = new NpgsqlCommand($"UPDATE dbkeepernet_lock SET expiration = current_timestamp at time zone 'UTC' + interval '{expirationMinutes} min' WHERE id = @id AND expiration < current_timestamp at time zone 'UTC'", npgsqlConnection))
            {
                var id = new NpgsqlParameter("@id", DbType.Int32) { Value = lockId };
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
            var npgsqlConnection = _databaseService.GetOpenConnection();
            using (var transaction = npgsqlConnection.BeginTransaction())
            using (var cmd = new NpgsqlCommand(@"UPDATE dbkeepernet_lock SET expiration = current_timestamp at time zone 'UTC' - interval '1 min' WHERE id = @id", npgsqlConnection))
            {
                var id = new NpgsqlParameter("@id", DbType.Int32) { Value = lockId };
                cmd.Parameters.Add(id);
                cmd.Transaction = transaction;
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }
    }
}
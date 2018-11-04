using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SQLite.Tests
{
    [TestFixture]
    public class SQLiteDatabaseLockTest: DatabaseLockTests
    {
        private const string ConnectionString = "Data source=sqlitetest.db3";

        protected override void Reset()
        {
            ExecuteSqlAndIgnoreException($"DELETE FROM dbkeepernet_lock WHERE id = {TestLockId}");
            ExecuteSqlAndIgnoreException($"INSERT INTO dbkeepernet_lock(id, description, expiration) VALUES({TestLockId}, 'Database upgrade mutex', datetime('now', 'utc', '-1 minutes'))");
        }

        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSQLite(ConnectionString)
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }
    }
}
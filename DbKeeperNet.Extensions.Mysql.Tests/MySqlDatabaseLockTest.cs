using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Mysql.Tests
{
    [TestFixture]
    public class MySqlDatabaseLockTest: DatabaseLockTests
    {
        protected override void Reset()
        {
            ExecuteSqlAndIgnoreException($"DELETE FROM dbkeepernet_lock WHERE id = {TestLockId}");
            ExecuteSqlAndIgnoreException($"INSERT INTO dbkeepernet_lock(id, description, expiration) VALUES({TestLockId}, 'Database upgrade mutex', DATE_ADD(UTC_TIMESTAMP(), INTERVAL -1 MINUTE))");
        }
        
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseMysql(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }
    }
}
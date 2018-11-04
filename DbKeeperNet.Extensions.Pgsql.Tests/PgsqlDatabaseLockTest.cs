using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Pgsql.Tests
{
    [TestFixture]
    public class PgsqlDatabaseLockTest: DatabaseLockTests
    {
        protected override void Reset()
        {
            ExecuteSqlAndIgnoreException($"DELETE FROM dbkeepernet_lock WHERE id = {TestLockId}");
            ExecuteSqlAndIgnoreException($"INSERT INTO dbkeepernet_lock(id, description, expiration) VALUES({TestLockId}, 'Database upgrade mutex', current_timestamp at time zone 'UTC' - interval '1 min')");
        }
        
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UsePgsql(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }
    }
}
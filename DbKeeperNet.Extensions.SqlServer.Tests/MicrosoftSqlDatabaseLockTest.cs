using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SqlServer.Tests
{
    [TestFixture]
    public class MicrosoftSqlDatabaseLockTest : DatabaseLockTests
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {

            configurationBuilder
                .UseSqlServer(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }

        protected override void Reset()
        {
            ExecuteSqlAndIgnoreException($"DELETE FROM dbkeepernet_lock WHERE id = {TestLockId}");
            ExecuteSqlAndIgnoreException($"INSERT INTO dbkeepernet_lock(id, description, expiration) VALUES({TestLockId}, 'Database upgrade mutex', DATEADD(mi, -1, GETUTCDATE()) )");
        }
    }
}
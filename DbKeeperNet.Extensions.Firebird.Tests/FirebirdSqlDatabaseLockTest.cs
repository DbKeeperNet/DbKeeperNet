using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Firebird.Tests
{
    [TestFixture]
    public class FirebirdSqlDatabaseLockTest: DatabaseLockTests
    {
        protected override void Reset()
        {
            ExecuteSqlAndIgnoreException($"DELETE FROM \"dbkeepernet_lock\" WHERE \"id\" = {TestLockId}");
            ExecuteSqlAndIgnoreException($"INSERT INTO \"dbkeepernet_lock\"(\"id\", \"description\", \"expiration\") VALUES({TestLockId}, 'Database upgrade mutex', DATEADD(minute, -1, CURRENT_TIMESTAMP))");
        }
        
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseFirebird(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }
    }
}
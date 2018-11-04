using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SQLite.Tests.Checkers
{
    [TestFixture]
    public class SQLiteUpdateStepExecutedCheckerTest : UpdateStepExecutedCheckerBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSQLite("Data Source=endtoend.db3")
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }

        protected override void Cleanup()
        {
            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_step");
            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_version");
            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_assembly");
            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_lock");
        }
    }
}
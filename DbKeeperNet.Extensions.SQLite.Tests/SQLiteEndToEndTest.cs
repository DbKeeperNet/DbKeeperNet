using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SQLite.Tests
{
    [TestFixture]
    public class SQLiteEndToEndTest : EndToEndTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSQLite("Data Source=endtoend.db3")
                .AddEmbeddedResourceScript(
                    "DbKeeperNet.Extensions.SQLite.Tests.SQLiteEndToEndTest.xml,DbKeeperNet.Extensions.SQLite.Tests")
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }

        public override void Setup()
        {
            base.Setup();

            Cleanup();
        }

        private void Cleanup()
        {
            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_step");
            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_version");
            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_assembly");
            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_lock");
            ExecuteSqlAndIgnoreException(@"DROP TABLE DbKeeperNet_SimpleDemo");
        }
    }
}
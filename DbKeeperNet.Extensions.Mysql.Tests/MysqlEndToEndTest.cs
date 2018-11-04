using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Mysql.Tests
{
    [TestFixture]
    public class MysqlEndToEndTest : EndToEndTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseMysql(ConnectionStrings.TestDatabase)
                .AddEmbeddedResourceScript(
                    "DbKeeperNet.Extensions.Mysql.Tests.MysqlEndToEndTest.xml,DbKeeperNet.Extensions.Mysql.Tests")
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }

        public override void Setup()
        {
            base.Setup();

            Cleanup();
        }

        public override void Shutdown()
        {
            Cleanup();

            base.Shutdown();
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
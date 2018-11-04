using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Pgsql.Tests
{
    [TestFixture]
    public class PgsqlEndToEndTest : EndToEndTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UsePgsql(ConnectionStrings.TestDatabase)
                .AddEmbeddedResourceScript(
                    "DbKeeperNet.Extensions.Pgsql.Tests.PgsqlEndToEndTest.xml,DbKeeperNet.Extensions.Pgsql.Tests")
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
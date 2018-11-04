using System.Threading;
using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Firebird.Tests
{
    [TestFixture]
    public class FirebirdEndToEndTest : EndToEndTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseFirebird(ConnectionStrings.TestDatabase)
                .AddEmbeddedResourceScript(
                    "DbKeeperNet.Extensions.Firebird.Tests.FirebirdEndToEndTest.xml,DbKeeperNet.Extensions.Firebird.Tests")
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }

        protected override void AssertThatTableExists(IDatabaseService connection)
        {
           ExecuteCommand(connection, @"select * from ""DbKeeperNet_SimpleDemo""");
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
            //Thread.Sleep(2000);

            // TODO: Doesn't work due to open connection
            //ExecuteSqlAndIgnoreException(@"DROP TRIGGER ""TR_dbkeepernet_step""");
            //ExecuteSqlAndIgnoreException(@"DROP TRIGGER ""TR_dbkeepernet_version""");
            //ExecuteSqlAndIgnoreException(@"DROP TRIGGER ""TR_dbkeepernet_assembly""");

            //ExecuteSqlAndIgnoreException(@"DROP GENERATOR ""GEN_dbkeepernet_step_id""");
            //ExecuteSqlAndIgnoreException(@"DROP GENERATOR ""GEN_dbkeepernet_version_id""");
            //ExecuteSqlAndIgnoreException(@"DROP GENERATOR ""GEN_dbkeepernet_assembly_id""");

            //ExecuteSqlAndIgnoreException(@"ALTER TABLE ""dbkeepernet_version"" DROP CONSTRAINT ""FK_dbknetver_dbknetasm_id""");
            //ExecuteSqlAndIgnoreException(@"ALTER TABLE ""dbkeepernet_step"" DROP CONSTRAINT ""FK_dbknetstep_dbknetver_id""");

            //ExecuteSqlAndIgnoreException(@"DROP TABLE ""dbkeepernet_step""");
            //ExecuteSqlAndIgnoreException(@"DROP TABLE ""dbkeepernet_version""");
            //ExecuteSqlAndIgnoreException(@"DROP TABLE ""dbkeepernet_assembly""");
            //ExecuteSqlAndIgnoreException(@"DROP TABLE ""dbkeepernet_lock""");

            //ExecuteSqlAndIgnoreException(@"DROP TABLE ""DbKeeperNet_SimpleDemo""");
            
            //Thread.Sleep(2000);
        }

    }
}
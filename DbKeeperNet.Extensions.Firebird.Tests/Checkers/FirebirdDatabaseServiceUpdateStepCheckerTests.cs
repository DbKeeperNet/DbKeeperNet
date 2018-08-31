using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Firebird.Tests.Checkers
{
    [TestFixture]
    public class FirebirdDatabaseServiceUpdateStepExecutedCheckerTests: UpdateStepExecutedCheckerBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseFirebird(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging();
        }

        protected override void Cleanup()
        {
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
        }
    }
}
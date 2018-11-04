using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SqlServer.Tests.Checkers
{
    [TestFixture]
    public class MicrosoftSqlDatabaseServiceUpdateStepExecutedCheckerTest : UpdateStepExecutedCheckerBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSqlServer(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }

        protected override void Cleanup()
        {
            ExecuteSqlAndIgnoreException(@"DROP PROCEDURE DbKeeperNetSetStepExecuted");
            ExecuteSqlAndIgnoreException(@"DROP PROCEDURE DbKeeperNetIsStepExecuted");
            ExecuteSqlAndIgnoreException(@"DROP PROCEDURE DbKeeperNetAcquireLock");
            ExecuteSqlAndIgnoreException(@"DROP PROCEDURE DbKeeperNetReleaseLock");


            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_step");
            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_version");
            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_assembly");
            ExecuteSqlAndIgnoreException(@"DROP TABLE dbkeepernet_lock");
        }
    }
}
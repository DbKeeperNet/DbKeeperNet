using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Pgsql.Tests.Checkers
{
    [TestFixture]
    public class PgSqlDatabaseServiceUpdateStepExecutedCheckerTests: UpdateStepExecutedCheckerBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UsePgsql(ConnectionStrings.TestDatabase)
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
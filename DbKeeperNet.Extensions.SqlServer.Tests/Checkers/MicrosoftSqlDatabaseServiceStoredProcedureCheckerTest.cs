using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SqlServer.Tests.Checkers
{
    [TestFixture]
    public class MicrosoftSqlDatabaseServiceStoredProcedureCheckerTest : StoredProcedureCheckerTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSqlServer(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }

        protected override void CreateStoredProcedure(string procedureName)
        {
            ExecuteSqlAndIgnoreException("create procedure {0} as select 1", procedureName);
        }

        protected override void DropStoredProcedure(string procedureName)
        {
            ExecuteSqlAndIgnoreException("drop procedure {0}", procedureName);
        }

    }
}
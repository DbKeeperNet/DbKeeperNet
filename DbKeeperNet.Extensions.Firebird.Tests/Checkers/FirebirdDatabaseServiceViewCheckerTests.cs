using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Firebird.Tests.Checkers
{
    [TestFixture]
    public class FirebirdDatabaseServiceViewCheckerTests: ViewCheckerTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseFirebird(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging();
        }

        protected override void CreateView(string viewName)
        {
            ExecuteSqlAndIgnoreException("create view \"{0}\" as select rdb$relation_name from rdb$relations where 1 = 0", viewName);
        }

        protected override void DropView(string viewName)
        {
            ExecuteSqlAndIgnoreException("drop view \"{0}\"", viewName);
        }

    }
}
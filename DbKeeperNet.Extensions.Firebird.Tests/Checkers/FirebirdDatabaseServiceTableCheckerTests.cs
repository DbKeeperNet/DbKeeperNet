using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Firebird.Tests.Checkers
{
    [TestFixture]
    public class FirebirdDatabaseServiceTableCheckerTests : TableCheckerTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseFirebird(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }

        protected override void CreateTable(string tableName)
        {
            ExecuteSqlAndIgnoreException(@"create table ""{0}"" (id int)", tableName);
        }

        protected override void DropTable(string tableName)
        {
            ExecuteSqlAndIgnoreException(@"drop table ""{0}""", tableName);
        }

    }
}
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Firebird.Tests.Checkers
{
    [TestFixture]
    public class FirebirdDatabaseServiceIndexCheckerTests: IndexCheckerTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseFirebird(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging();
        }

        protected override void CreateNamedIndex(string tableName, string indexName)
        {
            ExecuteSqlAndIgnoreException(@"create table ""{0}""(id int not null)", tableName);
            ExecuteSqlAndIgnoreException(@"CREATE INDEX ""{1}"" on ""{0}"" (id)", tableName, indexName);
        }

        protected override void DropNamedIndex(string tableName, string indexName)
        {
            ExecuteSqlAndIgnoreException(@"drop table ""{0}""", tableName);
        }
    }
}

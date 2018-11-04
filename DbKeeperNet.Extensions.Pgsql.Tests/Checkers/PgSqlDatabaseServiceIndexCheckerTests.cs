using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Pgsql.Tests.Checkers
{
    [TestFixture]
    public class PgSqlDatabaseServiceIndexCheckerTests: IndexCheckerTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UsePgsql(ConnectionStrings.TestDatabase)
                ;
            
            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
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

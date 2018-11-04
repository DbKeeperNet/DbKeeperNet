using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SqlServer.Tests.Checkers
{
    [TestFixture]
    public class MicrosoftSqlDatabaseServicePrimaryKeyCheckerTest : PrimaryKeyCheckerTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSqlServer(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }

        protected override void CreateNamedPrimaryKey(string tableName, string primaryKeyName)
        {
            ExecuteSqlAndIgnoreException("create table {0}(id int not null, CONSTRAINT {1} PRIMARY KEY (id))", tableName, primaryKeyName);
        }

        protected override void DropNamedPrimaryKey(string tableName, string primaryKeyName)
        {
            ExecuteSqlAndIgnoreException("drop table {0}", tableName);
        }
    }
}
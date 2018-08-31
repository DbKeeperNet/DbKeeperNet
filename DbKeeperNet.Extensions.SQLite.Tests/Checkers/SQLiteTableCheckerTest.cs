using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SQLite.Tests.Checkers
{
    [TestFixture]
    public class SQLiteTableCheckerTest : TableCheckerTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSQLite("Data Source=endtoend.db3")
                ;

            configurationBuilder.Services.AddLogging();
        }


        protected override void CreateTable(string tableName)
        {
            ExecuteSqlAndIgnoreException("create table {0}(c text)", tableName);
        }

        protected override void DropTable(string tableName)
        {
            ExecuteSqlAndIgnoreException("drop table {0}", tableName);
        }
    }
}
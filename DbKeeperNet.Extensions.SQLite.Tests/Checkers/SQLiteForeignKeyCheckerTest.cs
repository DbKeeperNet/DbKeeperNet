using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SQLite.Tests.Checkers
{
    [TestFixture]
    public class SQLiteForeignKeyCheckerTest : ForeignKeyCheckerTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSQLite("Data Source=endtoend.db3")
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }

        protected override void CreateNamedForeignKey(string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException("create table {0}(id integer not null, rec_id int, CONSTRAINT PK_{0} PRIMARY KEY (id), CONSTRAINT {1} FOREIGN KEY (rec_id) REFERENCES testing_fk(id))", tableName, foreignKeyName);
        }

        protected override void DropNamedForeignKey(string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException("drop table {0}", tableName);
        }
    }
}
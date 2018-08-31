using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Mysql.Tests.Checkers
{
    [TestFixture]
    public class MySqlDatabaseServiceForeignKeyCheckerTest : ForeignKeyCheckerTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseMysql(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging();
        }

        protected override void CreateNamedForeignKey(string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException("create table mysql_testing_fk(id int not null, CONSTRAINT PK_mysql_testing_fk PRIMARY KEY (id))");
            ExecuteSqlAndIgnoreException("CREATE TABLE {0}(rec_id int, CONSTRAINT {1} FOREIGN KEY (rec_id) REFERENCES mysql_testing_fk(id))", tableName, foreignKeyName);
        }

        protected override void DropNamedForeignKey(string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException("drop table {0}", tableName);
            ExecuteSqlAndIgnoreException("drop table mysql_testing_fk");
        }


    }
}
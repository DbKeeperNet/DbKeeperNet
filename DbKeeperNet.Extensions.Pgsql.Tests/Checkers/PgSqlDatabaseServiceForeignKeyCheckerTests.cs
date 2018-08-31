using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Pgsql.Tests.Checkers
{
    [TestFixture]
    public class PgSqlDatabaseServiceForeignKeyCheckerTests: ForeignKeyCheckerTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UsePgsql(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging();
        }

        protected override void CreateNamedForeignKey(string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException(@"create table ""{0}""(id int not null, rec_id int, CONSTRAINT ""PK_{0}"" PRIMARY KEY (id), CONSTRAINT ""{1}"" FOREIGN KEY (rec_id) REFERENCES ""{0}"" (id))", tableName, foreignKeyName);
        }

        protected override void DropNamedForeignKey(string tableName, string foreignKeyName)
        {
            ExecuteSqlAndIgnoreException(@"drop table ""{0}""", tableName);
        }
    }
}
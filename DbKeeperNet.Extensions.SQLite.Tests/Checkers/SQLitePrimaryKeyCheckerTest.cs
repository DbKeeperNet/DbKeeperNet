using System;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SQLite.Tests.Checkers
{
    [TestFixture]
    public class SQLitePrimaryKeyCheckerTest : PrimaryKeyCheckerTestBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSQLite("Data Source=endtoend.db3")
                ;

            configurationBuilder.Services.AddLogging();
        }

        [Test]
        public void TestPrimaryKeyNotExistsNullTableName()
        {
            Assert.Throws<ArgumentNullException>(() => TestPrimaryKeyExists("pk", null));
        }

        [Test]
        public void TestPrimaryKeyNotExistsEmptyTableName()
        {
            Assert.Throws<ArgumentNullException>(() => TestPrimaryKeyExists("pk", ""));
        }

        protected override void CreateNamedPrimaryKey(string tableName, string primaryKeyName)
        {
            ExecuteSqlAndIgnoreException("create table {0}(id integer not null, CONSTRAINT {1} PRIMARY KEY (id))", tableName, primaryKeyName);
        }

        protected override void DropNamedPrimaryKey(string tableName, string primaryKeyName)
        {
            ExecuteSqlAndIgnoreException("drop table {0}", tableName);
        }
    }
}
using System;
using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using System.Data.Common;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("pgsql")]
    public class PgSqlDatabaseServicesTests : DatabaseServiceTests<PgSqlDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"pgsql";

        public PgSqlDatabaseServicesTests()
            : base(APP_CONFIG_CONNECT_STRING)
        {
        }

        [SetUp]
        public void Startup()
        {
            CleanupDatabase();
        }

        [TearDown]
        public void Shutdown()
        {
            CleanupDatabase();
        }

        private void CleanupDatabase()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSqlAndIgnoreException(connectedService, "drop table pgsql_testing_table");
                ExecuteSqlAndIgnoreException(connectedService, "drop view pgsql_testing_view");
                ExecuteSqlAndIgnoreException(connectedService, "drop table pgsql_testing_ix");
                ExecuteSqlAndIgnoreException(connectedService, "drop table pgsql_testing_fk2; drop table pgsql_testing_fk");
            }
        }
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestProcedureNotExistsNullName()
        {
            TestStoredProcedureExists(null);
        }
        [Test]
        public void TestExecuteSql()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                connectedService.ExecuteSql("select 1");
            }
        }
        [Test]
        public void TestExecuteInvalidSqlStatement()
        {
            bool success = false;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                try
                {
                    connectedService.ExecuteSql("selectum magicum incorectum");
                }
                catch (DbException)
                {
                    success = true;
                }
            }

            Assert.That(success, Is.True);
        }
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestIndexNotExists()
        {
            TestIndexExists(null, null);
        }
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestForeignKeyNotExists()
        {
            TestForeignKeyExists(null, null);
        }
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestPrimaryKeyExists()
        {
            TestPrimaryKeyExists(null, null);
        }
    }
}

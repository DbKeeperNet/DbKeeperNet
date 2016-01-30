using System;
using System.Data.Common;
using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("pgsql")]
    public class PgSqlDatabaseServiceTests : DatabaseServiceTests<PgSqlDatabaseService>
    {
        private const string APP_CONFIG_CONNECT_STRING = @"pgsql";

        public PgSqlDatabaseServiceTests()
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
            }
        }

        [Test]
        public void TestProcedureNotExistsNullName()
        {
            Assert.Throws<NotSupportedException>(() => TestStoredProcedureExists(null));
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
    }
}

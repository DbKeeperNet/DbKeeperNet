using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using System.Data.Common;
using System.Diagnostics;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("mssql")]
    public class MsSqlDatabaseServiceTests: DatabaseServiceTests<MsSqlDatabaseService>
    {
        public MsSqlDatabaseServiceTests()
            : base(@"mssql")
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
                ExecuteSQLAndIgnoreException(connectedService, "drop table mssql_testing_table");
                ExecuteSQLAndIgnoreException(connectedService, "drop procedure mssql_testing_proc");
                ExecuteSQLAndIgnoreException(connectedService, "drop view mssql_testing_view");
                ExecuteSQLAndIgnoreException(connectedService, "drop table mssql_testing_ix");
                ExecuteSQLAndIgnoreException(connectedService, "drop table mssql_testing_fk");
                ExecuteSQLAndIgnoreException(connectedService, "drop table mssql_testing_pk");
                ExecuteSQLAndIgnoreException(connectedService, "drop table mssql_testing_tr");
            }
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

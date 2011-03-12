using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using System.Data.Common;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("mysql")]
    public class MySqlNetConnectorDatabaseServicesTests: DatabaseServiceTests<MySqlNetConnectorDatabaseService>
    {

        public MySqlNetConnectorDatabaseServicesTests()
            : base(@"mysql")
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
                ExecuteSQLAndIgnoreException(connectedService, "drop table mysql_testing_table");
                ExecuteSQLAndIgnoreException(connectedService, "drop view mysql_testing_view");
                ExecuteSQLAndIgnoreException(connectedService, "drop table mysql_testing_ix");
                ExecuteSQLAndIgnoreException(connectedService, "drop table mysql_testing_pk");
                ExecuteSQLAndIgnoreException(connectedService, "drop table mysql_testing_fk2; drop table mysql_testing_fk");
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
        public void TestForeignKeyNotExists()
        {
            TestForeignKeyExists("asddas", "asdsa");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestForeignKeyNotExistsNullName()
        {
            TestForeignKeyExists(null, null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestForeignKeyNotExistsEmptyName()
        {
            TestForeignKeyExists("", "");
        }
        [Test]
        public void TestForeignExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "create table mysql_testing_fk(id int not null, CONSTRAINT PK_mysql_testing_fk PRIMARY KEY (id))");
                ExecuteSQLAndIgnoreException(connectedService, "CREATE TABLE mysql_testing_fk2(rec_id int, CONSTRAINT FK_mysql_testing_fk FOREIGN KEY (rec_id) REFERENCES mysql_testing_fk(id))");
                
                Assert.That(TestForeignKeyExists("FK_mysql_testing_fk", "mysql_testing_fk2"), Is.True);
            }
        }
        [Test]
        public void TestPKNotExists()
        {
            TestPKExists("asddas", "asddsa");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPKNotExistsNullName()
        {
            TestPKExists(null, null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPKNotExistsEmptyName()
        {
            TestPKExists("", "");
        }
        [Test]
        public void TestPKExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "create table mysql_testing_pk(id int not null, CONSTRAINT PK_mysql_testing_pk PRIMARY KEY (id))");
                
                Assert.That(TestPKExists("PK_mysql_testing_pk", "mysql_testing_pk"), Is.True);
            }
        }
    }
}

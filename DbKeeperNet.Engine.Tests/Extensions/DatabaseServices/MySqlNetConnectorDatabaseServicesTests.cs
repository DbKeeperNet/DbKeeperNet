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
        protected override string ConnectionString
        {
            get { return @"mysql"; }
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
        public void TestTableExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "create table mysql_testing_table(c char)");
                Assert.That(TestTableExists("mysql_testing_table"), Is.True);
            }
        }
        [Test]
        public void TestTableNotExists()
        {
            Assert.That(TestTableExists("testing_table"), Is.False);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTableExistsNullName()
        {
            TestTableExists(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTableExistsEmptyName()
        {
            TestTableExists("");
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
        public void TestViewNotExists()
        {
            TestViewExists("asddas");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewNotExistsNullName()
        {
            TestViewExists(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewNotExistsEmptyName()
        {
            TestViewExists("");
        }
        [Test]
        public void TestViewExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "create view mysql_testing_view as select 1 as version");
                
                Assert.That(TestViewExists("mysql_testing_view"), Is.True);
            }
        }
        [Test]
        public void TestIndexNotExists()
        {
            TestIndexExists("asddas", "asddsa");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestIndexNotExistsNullName()
        {
            TestIndexExists(null, null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestIndexNotExistsEmptyName()
        {
            TestIndexExists("", "");
        }
        [Test]
        public void TestIndexExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "create table mysql_testing_ix(id int not null);CREATE INDEX IX_mysql_testing_ix on mysql_testing_ix (id)");
                
                Assert.That(TestIndexExists("IX_mysql_testing_ix", "mysql_testing_ix"), Is.True);
            }
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

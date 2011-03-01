using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Data.Common;
using DbKeeperNet.Engine.Extensions.DatabaseServices;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("sqlite")]
    public class SQLiteDatabaseServiceTests: DatabaseServiceTests<SQLiteDatabaseService>
    {
        protected override string ConnectionString
        {
            get { return @"sqlite"; }
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
                ExecuteSQLAndIgnoreException(connectedService, "drop table dbk_tst");
                ExecuteSQLAndIgnoreException(connectedService, "drop procedure sqlite_testing_proc");
                ExecuteSQLAndIgnoreException(connectedService, "drop view testing_view");
                ExecuteSQLAndIgnoreException(connectedService, "drop table testing_ix");
                ExecuteSQLAndIgnoreException(connectedService, "drop table testing_pk");
                ExecuteSQLAndIgnoreException(connectedService, "drop table testing_fk");
            }
        }

        [Test]
        public void TestTableExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                // we just need a table to check whether is exists, so don't care about
                // database failures
                ExecuteSQLAndIgnoreException(connectedService, "create table dbk_tst(c text)");
                
                Assert.That(TestTableExists("dbk_tst"), Is.True);
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
        public void TestProcedureNotExists()
        {
            TestStoredProcedureExists("asddas");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestProcedureNotExistsNullName()
        {
            TestStoredProcedureExists(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestProcedureNotExistsEmptyName()
        {
            TestStoredProcedureExists("");
        }
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestStoredProcedureExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "create procedure sqlite_testing_proc as select 1");
                
                Assert.That(TestStoredProcedureExists("sqlite_testing_proc"), Is.True);
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
                ExecuteSQLAndIgnoreException(connectedService, "create view testing_view as select 1 as version");
                
                Assert.That(TestViewExists("testing_view"), Is.True);
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
                ExecuteSQLAndIgnoreException(connectedService, "create table testing_ix(id int not null);CREATE INDEX IX_testing_ix on testing_ix (id)");
                
                Assert.That(TestIndexExists("IX_testing_ix", "testing_ix"), Is.True);
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
                ExecuteSQLAndIgnoreException(connectedService, "create table testing_fk(id integer not null, rec_id int, CONSTRAINT PK_testing_fk PRIMARY KEY (id), CONSTRAINT FK_testing_fk FOREIGN KEY (rec_id) REFERENCES testing_fk(id))");
                
                Assert.That(TestForeignKeyExists("FK_testing_fk", "testing_fk"), Is.True);
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
                ExecuteSQLAndIgnoreException(connectedService, "create table testing_pk(id integer not null, CONSTRAINT PK_testing_pk PRIMARY KEY (id))");
                
                Assert.That(TestPKExists("PK_testing_pk", "testing_pk"), Is.True);
            }
        }
    }
}

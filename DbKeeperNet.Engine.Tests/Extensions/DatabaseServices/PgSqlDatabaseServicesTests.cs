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
    [Category("pgsql")]
    public class PgSqlDatabaseServicesTests : DatabaseServiceTests<PgSqlDatabaseService>
    {
        public PgSqlDatabaseServicesTests()
            : base(@"pgsql")
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
                ExecuteSQLAndIgnoreException(connectedService, "drop table pgsql_testing_table");
                ExecuteSQLAndIgnoreException(connectedService, "drop view pgsql_testing_view");
                ExecuteSQLAndIgnoreException(connectedService, "drop table pgsql_testing_ix");
                ExecuteSQLAndIgnoreException(connectedService, "drop table pgsql_testing_pk");
                ExecuteSQLAndIgnoreException(connectedService, "drop table pgsql_testing_fk2; drop table pgsql_testing_fk");
            }
        }
        /*
        [Test]
        public void TestTableExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "create table pgsql_testing_table(c char)");
                Assert.That(TestTableExists("pgsql_testing_table"), Is.True);
            }

        }*/
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
                ExecuteSQLAndIgnoreException(connectedService, "create view pgsql_testing_view as select 1 as version");

                Assert.That(TestViewExists("pgsql_testing_view"), Is.True);
            }
        }
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
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
        [ExpectedException(typeof(NotSupportedException))]
        public void TestIndexExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "create table pgsql_testing_ix(id int not null);CREATE INDEX IX_pgsql_testing_ix on pgsql_testing_ix (id)");

                Assert.That(TestIndexExists("IX_pgsql_testing_ix", "pgsql_testing_ix"), Is.True);
            }
        }
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
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
        [ExpectedException(typeof(NotSupportedException))]
        public void TestForeignExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "create table pgsql_testing_fk(id int not null, CONSTRAINT PK_pgsql_testing_fk PRIMARY KEY (id))");
                ExecuteSQLAndIgnoreException(connectedService, "CREATE TABLE pgsql_testing_fk2(rec_id int, CONSTRAINT FK_pgsql_testing_fk FOREIGN KEY (rec_id) REFERENCES pgsql_testing_fk(id))");

                Assert.That(TestForeignKeyExists("FK_pgsql_testing_fk", "pgsql_testing_fk2"), Is.True);
            }
        }
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
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
        [ExpectedException(typeof(NotSupportedException))]
        public void TestPKExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                ExecuteSQLAndIgnoreException(connectedService, "create table pgsql_testing_pk(id int not null, CONSTRAINT PK_pgsql_testing_pk PRIMARY KEY (id))");

                Assert.That(TestPKExists("PK_pgsql_testing_pk", "pgsql_testing_pk"), Is.True);
            }
        }
    }
}

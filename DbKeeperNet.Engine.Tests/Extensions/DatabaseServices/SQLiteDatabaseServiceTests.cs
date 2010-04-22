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
    public class SQLiteDatabaseServiceTests
    {
        const string CONNECTION_STRING = "sqlite";
        #region Private helper methods
        private bool TestForeignKeyExists(string key, string table)
        {
            SQLiteDatabaseService service = new SQLiteDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.ForeignKeyExists(key, table);
            }
            return result;
        }
        private bool TestIndexExists(string index, string table)
        {
            SQLiteDatabaseService service = new SQLiteDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.IndexExists(index, table);
            }
            return result;
        }
        private bool TestPKExists(string index, string table)
        {
            SQLiteDatabaseService service = new SQLiteDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.PrimaryKeyExists(index, table);
            }
            return result;
        }
        private bool TestTableExists(string table)
        {
            SQLiteDatabaseService service = new SQLiteDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.TableExists(table);
            }
            return result;
        }
        private bool TestStoredProcedureExists(string procedure)
        {
            SQLiteDatabaseService service = new SQLiteDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.StoredProcedureExists(procedure);
            }
            return result;
        }
        private bool TestViewExists(string view)
        {
            SQLiteDatabaseService service = new SQLiteDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.ViewExists(view);
            }
            return result;
        }
        #endregion
        [Test]
        public void TestTableExists()
        {
            SQLiteDatabaseService service = new SQLiteDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a table to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table dbk_tst(c text)");
                }
                catch (DbException e)
                {
                }
                Assert.That(TestTableExists("dbk_tst"), Is.True);
                // cleanup table
                try
                {
                    connectedService.ExecuteSql("drop table dbk_tst");
                }
                catch (DbException)
                {
                }
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
            SQLiteDatabaseService service = new SQLiteDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create procedure sqlite_testing_proc as select 1");
                }
                catch (DbException)
                {
                }
                Assert.That(TestStoredProcedureExists("sqlite_testing_proc"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop procedure sqlite_testing_proc");
                }
                catch (DbException)
                {
                }
            }

        }
        [Test]
        public void TestExecuteSql()
        {
            SQLiteDatabaseService service = new SQLiteDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                connectedService.ExecuteSql("select 1");
            }
        }
        [Test]
        public void TestExecuteInvalidSqlStatement()
        {
            SQLiteDatabaseService service = new SQLiteDatabaseService();
            bool success = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
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
            SQLiteDatabaseService service = new SQLiteDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create view testing_view as select 1 as version");
                }
                catch (DbException)
                {
                }
                Assert.That(TestViewExists("testing_view"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop view testing_view");
                }
                catch (DbException)
                {
                }
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
            SQLiteDatabaseService service = new SQLiteDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table testing_ix(id int not null);CREATE INDEX IX_testing_ix on testing_ix (id)");
                }
                catch (DbException)
                {
                }
                Assert.That(TestIndexExists("IX_testing_ix", "testing_ix"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table testing_ix");
                }
                catch (DbException)
                {
                }
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
            SQLiteDatabaseService service = new SQLiteDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table testing_fk(id integer not null, rec_id int, CONSTRAINT PK_testing_fk PRIMARY KEY (id), CONSTRAINT FK_testing_fk FOREIGN KEY (rec_id) REFERENCES testing_fk(id))");
                }
                catch (DbException)
                {
                }
                Assert.That(TestForeignKeyExists("FK_testing_fk", "testing_fk"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table testing_fk");
                }
                catch (DbException)
                {
                }
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
            SQLiteDatabaseService service = new SQLiteDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table testing_pk(id integer not null, CONSTRAINT PK_testing_pk PRIMARY KEY (id))");
                }
                catch (DbException e)
                {
                }
                Assert.That(TestPKExists("PK_testing_pk", "testing_pk"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table testing_pk");
                }
                catch (DbException)
                {
                }
            }
        }
    }
}

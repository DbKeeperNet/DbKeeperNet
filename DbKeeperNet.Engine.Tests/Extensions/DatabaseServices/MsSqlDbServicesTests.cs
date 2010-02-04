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
    [Category("mssql")]
    public class MsSqlDatabaseServiceTests
    {
        const string CONNECTION_STRING = "mssql";
        #region Private helper methods
        private bool TestForeignKeyExists(string key, string table)
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.ForeignKeyExists(key, table);
            }
            return result;
        }
        private bool TestIndexExists(string index, string table)
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.IndexExists(index, table);
            }
            return result;
        }
        private bool TestPKExists(string index, string table)
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.PrimaryKeyExists(index, table);
            }
            return result;
        }
        private bool TestTableExists(string table)
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.TableExists(table);
            }
            return result;
        }
        private bool TestStoredProcedureExists(string procedure)
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.StoredProcedureExists(procedure);
            }
            return result;
        }
        private bool TestViewExists(string view)
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();
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
            MsSqlDatabaseService service = new MsSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a table to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table mssql_testing_table(c varchar)");
                }
                catch (DbException)
                {
                }
                Assert.That(TestTableExists("mssql_testing_table"), Is.True);
                // cleanup table
                try
                {
                    connectedService.ExecuteSql("drop table mssql_testing_table");
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
        public void TestStoredProcedureExists()
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create procedure mssql_testing_proc as select 1");
                }
                catch (DbException)
                {
                }
                Assert.That(TestStoredProcedureExists("mssql_testing_proc"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop procedure mssql_testing_proc");
                }
                catch (DbException)
                {
                }
            }

        }
        [Test]
        public void TestExecuteSql()
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                connectedService.ExecuteSql("select 1");
            }
        }
        [Test]
        public void TestExecuteInvalidSqlStatement()
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();
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
            MsSqlDatabaseService service = new MsSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create view mssql_testing_view as select 1 as version");
                }
                catch (DbException)
                {
                }
                Assert.That(TestViewExists("mssql_testing_view"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop view mssql_testing_view");
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
            MsSqlDatabaseService service = new MsSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table mssql_testing_ix(id int not null);CREATE INDEX IX_mssql_testing_ix on mssql_testing_ix (id)");
                }
                catch (DbException)
                {
                }
                Assert.That(TestIndexExists("IX_mssql_testing_ix", "mssql_testing_ix"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table mssql_testing_ix");
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
            MsSqlDatabaseService service = new MsSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table mssql_testing_fk(id int not null, rec_id int, CONSTRAINT PK_mssql_testing_fk PRIMARY KEY (id), CONSTRAINT FK_mssql_testing_fk FOREIGN KEY (rec_id) REFERENCES mssql_testing_fk(id))");
                }
                catch (DbException)
                {
                }
                Assert.That(TestForeignKeyExists("FK_mssql_testing_fk", "mssql_testing_fk"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table mssql_testing_fk");
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
            MsSqlDatabaseService service = new MsSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table mssql_testing_pk(id int not null, CONSTRAINT PK_mssql_testing_pk PRIMARY KEY (id))");
                }
                catch (DbException)
                {
                }
                Assert.That(TestPKExists("PK_mssql_testing_pk", "mssql_testing_pk"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table mssql_testing_pk");
                }
                catch (DbException)
                {
                }
            }
        }
    }
}

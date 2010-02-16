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
    public class PgSqlDatabaseServicesTests
    {
        const string CONNECTION_STRING = "pgsql";
        #region Private helper methods
        private bool TestForeignKeyExists(string key, string table)
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.ForeignKeyExists(key, table);
            }
            return result;
        }
        private bool TestIndexExists(string index, string table)
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.IndexExists(index, table);
            }
            return result;
        }
        private bool TestPKExists(string index, string table)
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.PrimaryKeyExists(index, table);
            }
            return result;
        }
        private bool TestTableExists(string table)
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.TableExists(table);
            }
            return result;
        }
        private bool TestStoredProcedureExists(string procedure)
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.StoredProcedureExists(procedure);
            }
            return result;
        }
        private bool TestViewExists(string view)
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();
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
            PgSqlDatabaseService service = new PgSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a table to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table pgsql_testing_table(c char)");
                }
                catch (DbException)
                {
                }
                Assert.That(TestTableExists("pgsql_testing_table"), Is.True);
                // cleanup table
                try
                {
                    connectedService.ExecuteSql("drop table pgsql_testing_table");
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
        public void TestProcedureNotExistsNullName()
        {
            TestStoredProcedureExists(null);
        }
        [Test]
        public void TestExecuteSql()
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                connectedService.ExecuteSql("select 1");
            }
        }
        [Test]
        public void TestExecuteInvalidSqlStatement()
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();
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
            PgSqlDatabaseService service = new PgSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create view pgsql_testing_view as select 1 as version");
                }
                catch (DbException)
                {
                }
                Assert.That(TestViewExists("pgsql_testing_view"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop view pgsql_testing_view");
                }
                catch (DbException)
                {
                }
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
            PgSqlDatabaseService service = new PgSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table pgsql_testing_ix(id int not null);CREATE INDEX IX_pgsql_testing_ix on pgsql_testing_ix (id)");
                }
                catch (DbException)
                {
                }
                Assert.That(TestIndexExists("IX_pgsql_testing_ix", "pgsql_testing_ix"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table pgsql_testing_ix");
                }
                catch (DbException)
                {
                }
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
            PgSqlDatabaseService service = new PgSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table pgsql_testing_fk(id int not null, CONSTRAINT PK_pgsql_testing_fk PRIMARY KEY (id))");
                    connectedService.ExecuteSql("CREATE TABLE pgsql_testing_fk2(rec_id int, CONSTRAINT FK_pgsql_testing_fk FOREIGN KEY (rec_id) REFERENCES pgsql_testing_fk(id))");
                }
                catch (DbException)
                {
                }
                Assert.That(TestForeignKeyExists("FK_pgsql_testing_fk", "pgsql_testing_fk2"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table pgsql_testing_fk2; drop table pgsql_testing_fk");
                }
                catch (DbException)
                {
                }
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
            PgSqlDatabaseService service = new PgSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table pgsql_testing_pk(id int not null, CONSTRAINT PK_pgsql_testing_pk PRIMARY KEY (id))");
                }
                catch (DbException)
                {
                }
                Assert.That(TestPKExists("PK_pgsql_testing_pk", "pgsql_testing_pk"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table pgsql_testing_pk");
                }
                catch (DbException)
                {
                }
            }
        }
    }
}

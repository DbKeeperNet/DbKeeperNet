using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;
using System.Data.Common;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    public class MySqlNetConnectorDatabaseServicesTests
    {
        const string CONNECTION_STRING = "mysql";
        #region Private helper methods
        private bool TestForeignKeyExists(string key, string table)
        {
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.ForeignKeyExists(key, table);
            }
            return result;
        }
        private bool TestIndexExists(string index, string table)
        {
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.IndexExists(index, table);
            }
            return result;
        }
        private bool TestPKExists(string index, string table)
        {
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.PrimaryKeyExists(index, table);
            }
            return result;
        }
        private bool TestTableExists(string table)
        {
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.TableExists(table);
            }
            return result;
        }
        private bool TestStoredProcedureExists(string procedure)
        {
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.StoredProcedureExists(procedure);
            }
            return result;
        }
        private bool TestViewExists(string view)
        {
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();
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
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a table to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table mysql_testing_table(c char)");
                }
                catch (DbException)
                {
                }
                Assert.That(TestTableExists("mysql_testing_table"), Is.True);
                // cleanup table
                try
                {
                    connectedService.ExecuteSql("drop table mysql_testing_table");
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
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                connectedService.ExecuteSql("select 1");
            }
        }
        [Test]
        public void TestExecuteInvalidSqlStatement()
        {
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();
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
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create view mysql_testing_view as select 1 as version");
                }
                catch (DbException)
                {
                }
                Assert.That(TestViewExists("mysql_testing_view"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop view mysql_testing_view");
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
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table mysql_testing_ix(id int not null);CREATE INDEX IX_mysql_testing_ix on mysql_testing_ix (id)");
                }
                catch (DbException)
                {
                }
                Assert.That(TestIndexExists("IX_mysql_testing_ix", "mysql_testing_ix"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table mysql_testing_ix");
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
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table mysql_testing_fk(id int not null, CONSTRAINT PK_mysql_testing_fk PRIMARY KEY (id)); CREATE TABLE mysql_testing_fk2(rec_id int, CONSTRAINT FK_mysql_testing_fk FOREIGN KEY (rec_id) REFERENCES mysql_testing_fk(id))");
                }
                catch (DbException)
                {
                }
                Assert.That(TestForeignKeyExists("FK_mysql_testing_fk", "mysql_testing_fk2"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table mysql_testing_fk2; drop table mysql_testing_fk");
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
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                // we just need a stored proc to check whether is exists, so don't care about
                // database failures
                try
                {
                    connectedService.ExecuteSql("create table mysql_testing_pk(id int not null, CONSTRAINT PK_mysql_testing_pk PRIMARY KEY (id))");
                }
                catch (DbException)
                {
                }
                Assert.That(TestPKExists("PK_mysql_testing_pk", "mysql_testing_pk"), Is.True);
                // cleanup procedure
                try
                {
                    connectedService.ExecuteSql("drop table mysql_testing_pk");
                }
                catch (DbException)
                {
                }
            }
        }
    }
}

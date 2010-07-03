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
    public class MsSqlDatabaseServiceTests
    {
        const string CONNECTION_STRING = "mssql";
        #region Private helper methods
        private static void LocalExecuteSQL(IDatabaseService service, string sql)
        {
            try
            {
                service.ExecuteSql(sql);
            }
            catch (DbException)
            {
                // Debug.WriteLine("Ignored DbException: ", e.ToString());
            }
        }
        private bool TestTriggerExists(string trigger)
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.TriggerExists(trigger);
            }
            return result;
        }
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
        public void Cleanup()
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "drop table mssql_testing_table");
                LocalExecuteSQL(connectedService, "drop procedure mssql_testing_proc");
                LocalExecuteSQL(connectedService, "drop view mssql_testing_view");
                LocalExecuteSQL(connectedService, "drop table mssql_testing_ix");
                LocalExecuteSQL(connectedService, "drop table mssql_testing_fk");
                LocalExecuteSQL(connectedService, "drop table mssql_testing_pk");
                LocalExecuteSQL(connectedService, "drop table mssql_testing_tr");
            }
        }
        [Test]
        public void TestTableExists()
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "create table mssql_testing_table(c varchar)");
                
                Assert.That(TestTableExists("mssql_testing_table"), Is.True);
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
                LocalExecuteSQL(connectedService, "create procedure mssql_testing_proc as select 1");
               
                Assert.That(TestStoredProcedureExists("mssql_testing_proc"), Is.True);
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
                LocalExecuteSQL(connectedService, "create view mssql_testing_view as select 1 as version");

                Assert.That(TestViewExists("mssql_testing_view"), Is.True);
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
                LocalExecuteSQL(connectedService, "create table mssql_testing_ix(id int not null);CREATE INDEX IX_mssql_testing_ix on mssql_testing_ix (id)");
                
                Assert.That(TestIndexExists("IX_mssql_testing_ix", "mssql_testing_ix"), Is.True);
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
                LocalExecuteSQL(connectedService, "create table mssql_testing_fk(id int not null, rec_id int, CONSTRAINT PK_mssql_testing_fk PRIMARY KEY (id), CONSTRAINT FK_mssql_testing_fk FOREIGN KEY (rec_id) REFERENCES mssql_testing_fk(id))");

                Assert.That(TestForeignKeyExists("FK_mssql_testing_fk", "mssql_testing_fk"), Is.True);
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
                LocalExecuteSQL(connectedService, "create table mssql_testing_pk(id int not null, CONSTRAINT PK_mssql_testing_pk PRIMARY KEY (id))");
                
                Assert.That(TestPKExists("PK_mssql_testing_pk", "mssql_testing_pk"), Is.True);
            }
        }

        [Test]
        public void TestTriggerNotExists()
        {
            TestTriggerExists("asddas");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTriggerNotExistsNullName()
        {
            TestTriggerExists(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTriggerNotExistsEmptyName()
        {
            TestTriggerExists("");
        }
        [Test]
        public void TestTriggerExists()
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "create table mssql_testing_tr (id numeric(9,0) not null, id2 numeric(9,0))");

                LocalExecuteSQL(connectedService, @"create trigger TR_mssql_testing_tr on mssql_testing_tr after insert               
            		  as
			            update mssql_testing_tr set id2 = id"
                    );

                Assert.That(TestTriggerExists("TR_mssql_testing_Tr"), Is.True);
            }
        }
    }
}

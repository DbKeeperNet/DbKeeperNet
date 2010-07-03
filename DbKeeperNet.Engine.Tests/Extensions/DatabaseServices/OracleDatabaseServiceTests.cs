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
    [Category("oracle")]
    public class OracleDatabaseServiceTests
    {
        const string CONNECTION_STRING = "oracle";
        #region Private helper methods

        private static void LocalExecuteSQL(IDatabaseService service, string sql)
        {
            try
            {
                service.ExecuteSql(sql);
            }
            catch (DbException)
            {
            //    Debug.WriteLine("Ignored DbException: ", e.ToString());
            }
        }
        private bool TestForeignKeyExists(string key, string table)
        {
            OracleDatabaseService service = new OracleDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.ForeignKeyExists(key, table);
            }
            return result;
        }
        private bool TestIndexExists(string index, string table)
        {
            OracleDatabaseService service = new OracleDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.IndexExists(index, table);
            }
            return result;
        }
        private bool TestPKExists(string index, string table)
        {
            OracleDatabaseService service = new OracleDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.PrimaryKeyExists(index, table);
            }
            return result;
        }
        private bool TestTriggerExists(string trigger)
        {
            OracleDatabaseService service = new OracleDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.TriggerExists(trigger);
            }
            return result;
        }
        private bool TestSequenceExists(string sequence)
        {
            OracleDatabaseService service = new OracleDatabaseService();
            bool result = false;

            using (OracleDatabaseService connectedService = (OracleDatabaseService) service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.SequenceExists(sequence);
            }
            return result;
        }
        private bool TestTableExists(string table)
        {
            OracleDatabaseService service = new OracleDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.TableExists(table);
            }
            return result;
        }
        private bool TestStoredProcedureExists(string procedure)
        {
            OracleDatabaseService service = new OracleDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.StoredProcedureExists(procedure);
            }
            return result;
        }
        private bool TestViewExists(string view)
        {
            OracleDatabaseService service = new OracleDatabaseService();
            bool result = false;

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                result = connectedService.ViewExists(view);
            }
            return result;
        }
        #endregion
        [TearDown]
        public void Cleanup()
        {
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "drop table \"ora_testing_table\"");
                LocalExecuteSQL(connectedService, "drop procedure \"ora_testing_proc\"");

                LocalExecuteSQL(connectedService, "drop table \"ora_testing_tv\"");
                LocalExecuteSQL(connectedService, "drop view \"ora_testing_view\"");

                LocalExecuteSQL(connectedService, "drop table ora_testing_ix");

                LocalExecuteSQL(connectedService, "drop table \"ora_testing_fk\"");
                LocalExecuteSQL(connectedService, "drop table \"ora_testing_pk\"");
                LocalExecuteSQL(connectedService, "drop table \"ora_testing_tr\"");

                LocalExecuteSQL(connectedService, "drop sequence \"ora_testing_seq\"");
            }
        }
        [Test]
        public void TestTableExists()
        {
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "create table \"ora_testing_table\"(c varchar(2))");
                
                Assert.That(TestTableExists("ora_testing_table"), Is.True);
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
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "create procedure \"ora_testing_proc\" as select 1");
                
                Assert.That(TestStoredProcedureExists("ora_testing_proc"), Is.True);
            }
        }
        [Test]
        public void TestExecuteSql()
        {
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                connectedService.ExecuteSql("select sysdate from dual");
            }
        }
        [Test]
        public void TestExecuteInvalidSqlStatement()
        {
            OracleDatabaseService service = new OracleDatabaseService();
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
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "create table \"ora_testing_tv\" (c varchar(2))");
                LocalExecuteSQL(connectedService, "create view \"ora_testing_view\" as select * from \"ora_testing_tv\"");
                
                Assert.That(TestViewExists("ora_testing_view"), Is.True);
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
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "create table \"ora_testing_ix\"(id int not null)");
                LocalExecuteSQL(connectedService, "CREATE INDEX \"IX_ora_testing_ix\" on \"ora_testing_ix\" (id)");
                
                Assert.That(TestIndexExists("IX_ora_testing_ix", "ora_testing_ix"), Is.True);
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
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "create table \"ora_testing_fk\"(id numeric(9,0) not null, rec_id numeric(9,0), CONSTRAINT \"PK_ora_testing_fk\" PRIMARY KEY (id), CONSTRAINT \"FK_ora_testing_fk\" FOREIGN KEY (rec_id) REFERENCES \"ora_testing_fk\"(id))");
                
                Assert.That(TestForeignKeyExists("FK_ora_testing_fk", "ora_testing_fk"), Is.True);
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
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "create table \"ora_testing_pk\"(id numeric(9,0) not null, CONSTRAINT \"PK_ora_testing_pk\" PRIMARY KEY (id))");

                Assert.That(TestPKExists("PK_ora_testing_pk", "ora_testing_pk"), Is.True);
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
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "create table \"ora_testing_tr\"(id numeric(9,0) not null, id2 numeric(9,0))");

                LocalExecuteSQL(connectedService, @"create trigger ""TR_ora_testing_tr"" before insert on ""ora_testing_tr""              
            		  for each row 
			            begin  
                            select :NEW.ID into :NEW.ID2 from dual;
			            end;"
                    );

                Assert.That(TestTriggerExists("TR_ora_testing_tr"), Is.True);
            }
        }
        [Test]
        public void TestSequenceNotExists()
        {
            TestSequenceExists("asddas");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSequenceNotExistsNullName()
        {
            TestSequenceExists(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSequenceNotExistsEmptyName()
        {
            TestSequenceExists("");
        }
        [Test]
        public void TestSequenceExists()
        {
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                LocalExecuteSQL(connectedService, "create sequence \"ora_testing_seq\"");
                
                Assert.That(TestSequenceExists("ora_testing_seq"), Is.True);
            }
        }
    }
}

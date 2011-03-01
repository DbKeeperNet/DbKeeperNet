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
    public class MsSqlDatabaseServiceTests: DatabaseServiceTests<MsSqlDatabaseService>
    {
        protected override string ConnectionString
        {
            get
            {
                return @"mssql";
            }
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
                ExecuteSQLAndIgnoreException(connectedService, "drop table mssql_testing_table");
                ExecuteSQLAndIgnoreException(connectedService, "drop procedure mssql_testing_proc");
                ExecuteSQLAndIgnoreException(connectedService, "drop view mssql_testing_view");
                ExecuteSQLAndIgnoreException(connectedService, "drop table mssql_testing_ix");
                ExecuteSQLAndIgnoreException(connectedService, "drop table mssql_testing_fk");
                ExecuteSQLAndIgnoreException(connectedService, "drop table mssql_testing_pk");
                ExecuteSQLAndIgnoreException(connectedService, "drop table mssql_testing_tr");
            }
        }
        [Test]
        public void TestTableExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateTable(connectedService);
                
                Assert.That(TestTableExists("mssql_testing_table"), Is.True);
            }

        }

        private static void CreateTable(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table mssql_testing_table(c varchar)");
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
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateStoredProcedure(connectedService);
               
                Assert.That(TestStoredProcedureExists("mssql_testing_proc"), Is.True);
            }

        }

        private static void CreateStoredProcedure(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create procedure mssql_testing_proc as select 1");
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
                CreateView(connectedService);

                Assert.That(TestViewExists("mssql_testing_view"), Is.True);
            }
        }

        private static void CreateView(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create view mssql_testing_view as select 1 as version");
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
                CreateNamedIndex(connectedService);
                
                Assert.That(TestIndexExists("IX_mssql_testing_ix", "mssql_testing_ix"), Is.True);
            }
        }

        private static void CreateNamedIndex(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table mssql_testing_ix(id int not null);CREATE INDEX IX_mssql_testing_ix on mssql_testing_ix (id)");
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
                CreateNamedForeignKey(connectedService);

                Assert.That(TestForeignKeyExists("FK_mssql_testing_fk", "mssql_testing_fk"), Is.True);
            }
        }

        private static void CreateNamedForeignKey(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table mssql_testing_fk(id int not null, rec_id int, CONSTRAINT PK_mssql_testing_fk PRIMARY KEY (id), CONSTRAINT FK_mssql_testing_fk FOREIGN KEY (rec_id) REFERENCES mssql_testing_fk(id))");
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
                CreateNamedPrimaryKey(connectedService);
                
                Assert.That(TestPKExists("PK_mssql_testing_pk", "mssql_testing_pk"), Is.True);
            }
        }

        private static void CreateNamedPrimaryKey(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table mssql_testing_pk(id int not null, CONSTRAINT PK_mssql_testing_pk PRIMARY KEY (id))");
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
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateDatabaseTrigger(connectedService);

                Assert.That(TestTriggerExists("TR_mssql_testing_Tr"), Is.True);
            }
        }

        private static void CreateDatabaseTrigger(IDatabaseService connectedService)
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table mssql_testing_tr (id numeric(9,0) not null, id2 numeric(9,0))");

            ExecuteSQLAndIgnoreException(connectedService, @"create trigger TR_mssql_testing_tr on mssql_testing_tr after insert               
            		  as
			            update mssql_testing_tr set id2 = id"
                );
        }
    }
}

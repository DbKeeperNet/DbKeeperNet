using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine.Tests.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServicePrimaryKeyTests<T>: DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
        private const string TABLE_NAME = @"testing_table_for_pk";
        private const string PRIMARY_KEY_NAME = @"PK_testing_table_for_pk";

        protected DatabaseServicePrimaryKeyTests(string connectString)
            : base(connectString)
        {
        }

        [SetUp]
        public void SetUp()
        {
            Cleanup();
        }

        [TearDown]
        public void Shutdown()
        {
            Cleanup();
        }

        private void Cleanup()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                DropNamedPrimaryKey(connectedService, TABLE_NAME, PRIMARY_KEY_NAME);
            }
        }

        [Test]
        public void TestPKNotExists()
        {
            TestPKExists(TABLE_NAME, PRIMARY_KEY_NAME);
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
                CreateNamedPrimaryKey(connectedService, TABLE_NAME, PRIMARY_KEY_NAME);
                
                Assert.That(TestPKExists(PRIMARY_KEY_NAME, TABLE_NAME), Is.True);
            }
        }

        protected abstract void CreateNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName);
        protected abstract void DropNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName);
        /*
        {
            ExecuteSQLAndIgnoreException(connectedService, "create table mssql_testing_pk(id int not null, CONSTRAINT PK_mssql_testing_pk PRIMARY KEY (id))");
        }
        */
    }
}

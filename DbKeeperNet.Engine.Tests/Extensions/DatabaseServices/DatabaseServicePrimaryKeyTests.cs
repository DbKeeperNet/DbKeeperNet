using System;
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
        public void TestPrimaryKeyDoesNotExists()
        {
            TestPrimaryKeyExists(TABLE_NAME, PRIMARY_KEY_NAME);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPrimaryKeyNotExistsNullName()
        {
            TestPrimaryKeyExists(null, null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPrimaryKeyNotExistsEmptyName()
        {
            TestPrimaryKeyExists("", "");
        }
        [Test]
        public void TestPrimaryKeyExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateNamedPrimaryKey(connectedService, TABLE_NAME, PRIMARY_KEY_NAME);
                
                Assert.That(TestPrimaryKeyExists(PRIMARY_KEY_NAME, TABLE_NAME), Is.True);
            }
        }

        protected abstract void CreateNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName);
        protected abstract void DropNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName);
    }
}

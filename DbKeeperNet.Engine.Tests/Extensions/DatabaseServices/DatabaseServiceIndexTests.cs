using System;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceIndexTests<T>: DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
        private const string TABLE_NAME = "testing_table_ix";
        private const string INDEX_NAME = "IX_testing_table_ix";

        protected DatabaseServiceIndexTests(string connectionString)
            : base(connectionString)
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
                DropNamedIndex(connectedService, TABLE_NAME, INDEX_NAME);
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
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateNamedIndex(connectedService, TABLE_NAME, INDEX_NAME);

                Assert.That(TestIndexExists(INDEX_NAME, TABLE_NAME), Is.True);
            }
        }

        protected abstract void CreateNamedIndex(IDatabaseService connectedService, string tableName, string indexName);
        protected abstract void DropNamedIndex(IDatabaseService connectedService, string tableName, string indexName);
    }
}

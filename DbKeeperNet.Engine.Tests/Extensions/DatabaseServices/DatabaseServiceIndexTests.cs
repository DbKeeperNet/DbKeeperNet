using System;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceIndexTests<T>: DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
        private const string TABLE_NAME = "testing_table_ix";
        private const string INDEX_NAME = "IX_testing_table_ix";
		private const string UNKNOWN_INDEX_NAME = "IX_testing_table_unknown_ix";

        protected DatabaseServiceIndexTests(string connectionString)
            : base(connectionString)
        {
        }

        [SetUp]
        public void SetUp()
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

        [TearDown]
        public void Shutdown()
        {
            Cleanup();
        }
		
        [Test]
        public void TestIndexNotExists()
        {
        	CreateTestIndexInDatabase();

            TestIndexExists(UNKNOWN_INDEX_NAME, TABLE_NAME);
        }

        [Test]
        public void TestIndexNotExistsNullName()
        {
            Assert.Throws<ArgumentNullException>(() => TestIndexExists(null, null));
        }

        [Test]
        public void TestIndexNotExistsEmptyName()
        {
            Assert.Throws<ArgumentNullException>(() => TestIndexExists("", ""));
        }

        [Test]
        public void TestIndexExists()
        {
        	CreateTestIndexInDatabase();

        	Assert.That(TestIndexExists(INDEX_NAME, TABLE_NAME), Is.True);
        }

    	private void CreateTestIndexInDatabase()
    	{
    		using (IDatabaseService connectedService = CreateConnectedDbService())
    		{
    			CreateNamedIndex(connectedService, TABLE_NAME, INDEX_NAME);
    		}
    	}

		protected bool TestIndexExists(string index, string table)
		{
			bool result;

			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				result = connectedService.IndexExists(index, table);
			}
			return result;
		}

    	protected abstract void CreateNamedIndex(IDatabaseService connectedService, string tableName, string indexName);
        protected abstract void DropNamedIndex(IDatabaseService connectedService, string tableName, string indexName);
    }
}

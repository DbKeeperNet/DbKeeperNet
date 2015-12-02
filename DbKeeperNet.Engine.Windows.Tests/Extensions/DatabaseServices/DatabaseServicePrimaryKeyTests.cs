using System;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServicePrimaryKeyTests<T>: DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
        private const string TABLE_NAME = @"testing_table_for_pk";
        private const string PRIMARY_KEY_NAME = @"PK_testing_table_for_pk";
		private const string UNKNOWN_PRIMARY_KEY_NAME = @"PK_testing_table_for_unknown_pk";

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

        [Test]
        public void TestPrimaryKeyDoesNotExists()
        {
			CreateTestPrimaryKeyInDatabase();

			Assert.That(TestPrimaryKeyExists(TABLE_NAME, UNKNOWN_PRIMARY_KEY_NAME), Is.False);
        }

        [Test]
        public void TestPrimaryKeyNotExistsNullName()
        {
            Assert.Throws<ArgumentNullException>(() => TestPrimaryKeyExists(null, null));
        }

        [Test]
        public void TestPrimaryKeyNotExistsEmptyName()
        {
            Assert.Throws<ArgumentNullException>(() => TestPrimaryKeyExists("", ""));
        }

        [Test]
        public void TestPrimaryKeyExists()
        {
        	CreateTestPrimaryKeyInDatabase();

        	Assert.That(TestPrimaryKeyExists(PRIMARY_KEY_NAME, TABLE_NAME), Is.True);
        }

    	private void CreateTestPrimaryKeyInDatabase()
    	{
    		using (IDatabaseService connectedService = CreateConnectedDbService())
    		{
    			CreateNamedPrimaryKey(connectedService, TABLE_NAME, PRIMARY_KEY_NAME);
    		}
    	}

    	protected abstract void CreateNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName);
        protected abstract void DropNamedPrimaryKey(IDatabaseService connectedService, string tableName, string primaryKeyName);

    	private bool TestPrimaryKeyExists(string index, string table)
		{
			bool result;

			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				result = connectedService.PrimaryKeyExists(index, table);
			}
			return result;
		}


		private void Cleanup()
		{
			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				DropNamedPrimaryKey(connectedService, TABLE_NAME, PRIMARY_KEY_NAME);
			}
		}

    }
}

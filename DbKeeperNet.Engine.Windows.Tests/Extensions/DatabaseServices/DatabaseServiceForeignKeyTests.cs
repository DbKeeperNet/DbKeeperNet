using System;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceForeignKeyTests<T> : DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
        private const string TABLE_NAME = @"testing_tb_fk";
        private const string FOREIGN_KEY_NAME = @"FK_testing_tb_fk";
		private const string UNKNOWN_FOREIGN_KEY_NAME = @"FK_testing_tb_unknown_fk";

        protected DatabaseServiceForeignKeyTests(string connectionString)
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
		
        [Test]
        public void TestForeignKeyNotExists()
        {
        	CreateTestForeignKeyInDatabase();

            TestForeignKeyExists(UNKNOWN_FOREIGN_KEY_NAME, TABLE_NAME);
        }

        [Test]
        public void TestForeignKeyNotExistsNullName()
        {
            Assert.Throws<ArgumentNullException>(() => TestForeignKeyExists(null, null));
        }

        [Test]
        public void TestForeignKeyNotExistsEmptyName()
        {
            Assert.Throws<ArgumentNullException>(() => TestForeignKeyExists("", ""));
        }

        [Test]
        public void TestForeignKeyExists()
        {
        	CreateTestForeignKeyInDatabase();

        	Assert.That(TestForeignKeyExists(FOREIGN_KEY_NAME, TABLE_NAME), Is.True);
        }

    	private void CreateTestForeignKeyInDatabase()
    	{
    		using (IDatabaseService connectedService = CreateConnectedDbService())
    		{

    			CreateNamedForeignKey(connectedService, TABLE_NAME, FOREIGN_KEY_NAME);
    		}
    	}

    	protected bool TestForeignKeyExists(string key, string table)
		{
			bool result;

			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				result = connectedService.ForeignKeyExists(key, table);
			}
			return result;
		}

        protected abstract void CreateNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName);
        protected abstract void DropNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName);
		
		private void Cleanup()
		{
			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				DropNamedForeignKey(connectedService, TABLE_NAME, FOREIGN_KEY_NAME);
			}
		}
    }
}

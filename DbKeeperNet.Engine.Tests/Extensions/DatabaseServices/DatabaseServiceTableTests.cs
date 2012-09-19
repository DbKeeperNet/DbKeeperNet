using System;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceTableTests<T>: DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
    	private const string UNKNOWN_TABLE_NAME = @"unknown_table_name";
    	private const string TABLE_NAME = @"testing_table_name";

        protected DatabaseServiceTableTests(string connectString)
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
        public void TestTableExists()
        {
        	CreateTestTableInDatabase();

        	Assert.That(TestTableExists(TABLE_NAME), Is.True);
        }
		
    	[Test]
        public void TestTableNotExists()
        {
			CreateTestTableInDatabase();

            Assert.That(TestTableExists(UNKNOWN_TABLE_NAME), Is.False);
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
            TestTableExists(String.Empty);
        }


		private void CreateTestTableInDatabase()
		{
			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				CreateTable(connectedService, TABLE_NAME);
			}
		}

		private bool TestTableExists(string table)
		{
			bool result;

			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				result = connectedService.TableExists(table);
			}
			return result;
		}

		protected abstract void CreateTable(IDatabaseService connectedService, string tableName);
		protected abstract void DropTable(IDatabaseService connectedService, string tableName);


		private void Cleanup()
		{
			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				DropTable(connectedService, TABLE_NAME);
			}
		}
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceTableTests<T>: DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
        private const string TABLE_NAME = @"testing_table_name";

        protected DatabaseServiceTableTests(string connectString)
            : base(connectString)
        {
        }

        private bool TestTableExists(string table)
        {
            bool result = false;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.TableExists(table);
            }
            return result;
        }

        protected abstract void CreateTable(IDatabaseService connectedService, string tableName);
        protected abstract void DropTable(IDatabaseService connectedService, string tableName);

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
                DropTable(connectedService, TABLE_NAME);
            }
        }
        [Test]
        public void TestTableExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateTable(connectedService, TABLE_NAME);

                Assert.That(TestTableExists(TABLE_NAME), Is.True);
            }
        }

        [Test]
        public void TestTableNotExists()
        {
            Assert.That(TestTableExists(TABLE_NAME), Is.False);
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

    }
}

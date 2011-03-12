using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceForeignKeyTests<T> : DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
        private const string TABLE_NAME = @"testing_tb_fk";
        private const string FOREIGN_KEY_NAME = @"FK_testing_tb_fk";

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

        private void Cleanup()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                DropNamedForeignKey(connectedService, TABLE_NAME, FOREIGN_KEY_NAME);
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
        public void TestForeignKeyExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                
                CreateNamedForeignKey(connectedService, TABLE_NAME, FOREIGN_KEY_NAME);

                Assert.That(TestForeignKeyExists(FOREIGN_KEY_NAME, TABLE_NAME), Is.True);
            }
        }

        protected abstract void CreateNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName);
        protected abstract void DropNamedForeignKey(IDatabaseService connectedService, string tableName, string foreignKeyName);
        
    }
}

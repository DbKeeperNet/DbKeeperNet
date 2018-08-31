using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Checkers
{
    public abstract class PrimaryKeyCheckerTestBase : TestBase
    {
        private const string TABLE_NAME = @"testing_table_for_pk";
        private const string PRIMARY_KEY_NAME = @"PK_testing_table_for_pk";
        private const string UNKNOWN_PRIMARY_KEY_NAME = @"PK_testing_table_for_unknown_pk";

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            Cleanup();
        }

        [TearDown]
        public override void Shutdown()
        {
            Cleanup();

            base.Shutdown();
        }

        [Test]
        public void TestPrimaryKeyDoesNotExists()
        {
            CreateTestPrimaryKeyInDatabase();

            Assert.That(TestPrimaryKeyExists(UNKNOWN_PRIMARY_KEY_NAME, TABLE_NAME), Is.False);
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
            CreateNamedPrimaryKey(TABLE_NAME, PRIMARY_KEY_NAME);

        }

        protected abstract void CreateNamedPrimaryKey(string tableName, string primaryKeyName);
        protected abstract void DropNamedPrimaryKey(string tableName, string primaryKeyName);

        protected bool TestPrimaryKeyExists(string index, string table)
        {
            var checker = DefaultScope.ServiceProvider.GetService<IDatabaseServicePrimaryKeyChecker>();

            var result = checker.Exists(index, table);

            return result;
        }


        private void Cleanup()
        {
            DropNamedPrimaryKey(TABLE_NAME, PRIMARY_KEY_NAME);
        }

    }
}
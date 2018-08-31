using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Checkers
{
    public abstract class ForeignKeyCheckerTestBase : TestBase
    {
        private const string TABLE_NAME = @"testing_tb_fk";
        private const string FOREIGN_KEY_NAME = @"FK_testing_tb_fk_rec_id";
        private const string UNKNOWN_FOREIGN_KEY_NAME = @"FK_testing_tb_unknown_fk";

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
        public void TestForeignKeyNotExists()
        {
            CreateTestForeignKeyInDatabase();

            Assert.That(TestForeignKeyExists(UNKNOWN_FOREIGN_KEY_NAME, TABLE_NAME), Is.False);
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
            CreateNamedForeignKey(TABLE_NAME, FOREIGN_KEY_NAME);
        }

        protected bool TestForeignKeyExists(string key, string table)
        {
            bool result;

            var checker = DefaultScope.ServiceProvider.GetService<IDatabaseServiceForeignKeyChecker>();
            result = checker.Exists(key, table);

            return result;
        }

        protected abstract void CreateNamedForeignKey(string tableName, string foreignKeyName);
        protected abstract void DropNamedForeignKey(string tableName, string foreignKeyName);

        private void Cleanup()
        {
            DropNamedForeignKey(TABLE_NAME, FOREIGN_KEY_NAME);
        }
    }
}
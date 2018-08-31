using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Checkers
{
    public abstract class IndexCheckerTestBase : TestBase
    {
        private const string TABLE_NAME = "testing_table_ix";
        private const string INDEX_NAME = "IX_testing_table_ix";
        private const string UNKNOWN_INDEX_NAME = "IX_testing_table_unknown_ix";

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

        private void Cleanup()
        {
            DropNamedIndex(TABLE_NAME, INDEX_NAME);
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
                CreateNamedIndex(TABLE_NAME, INDEX_NAME);
         
        }

        protected bool TestIndexExists(string index, string table)
        {
            var checker = DefaultScope.ServiceProvider.GetService<IDatabaseServiceIndexChecker>();

            var result = checker.Exists(index, table);
            
            return result;
        }

        protected abstract void CreateNamedIndex(string tableName, string indexName);
        protected abstract void DropNamedIndex(string tableName, string indexName);
    }
}
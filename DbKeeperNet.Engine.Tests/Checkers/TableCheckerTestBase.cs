using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Checkers
{
    public abstract class TableCheckerTestBase : TestBase
    {
        private const string UNKNOWN_TABLE_NAME = @"unknown_table_name";
        private const string TABLE_NAME = @"testing_table_name";

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
        public void TestTableExistsNullName()
        {
            Assert.Throws<ArgumentNullException>(() => TestTableExists(null));
        }

        [Test]
        public void TestTableExistsEmptyName()
        {
            Assert.Throws<ArgumentNullException>(() => TestTableExists(String.Empty));
        }


        private void CreateTestTableInDatabase()
        {
         
                CreateTable(TABLE_NAME);
         
        }

        private bool TestTableExists(string table)
        {
            var checker = DefaultScope.ServiceProvider.GetService<IDatabaseServiceTableChecker>();

            var result = checker.Exists(table);

            return result;
        }

        protected abstract void CreateTable(string tableName);

        protected abstract void DropTable(string tableName);


        private void Cleanup()
        {
                DropTable( TABLE_NAME);
         
        }
    }
}
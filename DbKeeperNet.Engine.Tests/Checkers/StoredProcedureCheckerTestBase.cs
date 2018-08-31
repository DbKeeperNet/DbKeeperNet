using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Checkers
{
    public abstract class StoredProcedureCheckerTestBase : TestBase
    {
        private const string STORED_PROCEDURE_NAME = @"testing_stored_procedure";

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
        public void TestProcedureNotExists()
        {
            Assert.That(TestStoredProcedureExists(STORED_PROCEDURE_NAME), Is.False);
        }

        [Test]
        public void TestProcedureNotExistsNullName()
        {
            Assert.Throws<ArgumentNullException>(() => TestStoredProcedureExists(null));
        }

        [Test]
        public void TestProcedureNotExistsEmptyName()
        {
            Assert.Throws<ArgumentNullException>(() => TestStoredProcedureExists(String.Empty));
        }

        [Test]
        public void TestStoredProcedureExists()
        {
            
            CreateStoredProcedure(STORED_PROCEDURE_NAME);
            

            Assert.That(TestStoredProcedureExists(STORED_PROCEDURE_NAME), Is.True);
        }

        protected abstract void CreateStoredProcedure(string procedureName);
        protected abstract void DropStoredProcedure(string procedureName);

        protected bool TestStoredProcedureExists(string procedure)
        {
            var checker = DefaultScope.ServiceProvider.GetService<IDatabaseServiceStoredProcedureChecker>();

            var result = checker.Exists(procedure);

            return result;
        }


        private void Cleanup()
        {
            DropStoredProcedure(STORED_PROCEDURE_NAME);
        }

    }
}
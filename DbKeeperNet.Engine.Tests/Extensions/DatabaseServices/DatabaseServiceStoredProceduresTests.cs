using System;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceStoredProceduresTests<T>: DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
        private const string STORED_PROCEDURE_NAME = @"testing_stored_procedure";

        protected DatabaseServiceStoredProceduresTests(string connectString)
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
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
            	CreateStoredProcedure(connectedService, STORED_PROCEDURE_NAME);
            }

        	Assert.That(TestStoredProcedureExists(STORED_PROCEDURE_NAME), Is.True);
        }

        protected abstract void CreateStoredProcedure(IDatabaseService connectedService, string procedureName);
        protected abstract void DropStoredProcedure(IDatabaseService connectedService, string procedureName);


		private void Cleanup()
		{
			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				DropStoredProcedure(connectedService, STORED_PROCEDURE_NAME);
			}
		}
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceTriggerTests<T>: DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
        private const string TRIGGER_NAME = @"TR_some_testing";

        protected DatabaseServiceTriggerTests(string connectionString)
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
                DropDatabaseTrigger(connectedService, TRIGGER_NAME);
            }
        }

        [Test]
        public void TestTriggerNotExists()
        {
            TestTriggerExists("asddas");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTriggerNotExistsNullName()
        {
            TestTriggerExists(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTriggerNotExistsEmptyName()
        {
            TestTriggerExists("");
        }
        [Test]
        public void TestTriggerExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateDatabaseTrigger(connectedService, TRIGGER_NAME);

                Assert.That(TestTriggerExists(TRIGGER_NAME), Is.True);
            }
        }

        protected abstract void CreateDatabaseTrigger(IDatabaseService connectedService, string triggerName);
        protected abstract void DropDatabaseTrigger(IDatabaseService connectedService, string triggerName);
    }
}

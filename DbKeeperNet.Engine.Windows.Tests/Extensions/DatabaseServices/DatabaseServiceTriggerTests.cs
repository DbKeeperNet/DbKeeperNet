using System;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceTriggerTests<T>: DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
    	private const string UNKNOWN_TRIGGER_NAME = @"TR_some_unknown_testing";
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
		
        [Test]
        public void TestTriggerNotExists()
        {
			CreateTestTriggerInDatabase();

            Assert.That(TestTriggerExists(UNKNOWN_TRIGGER_NAME), Is.False);
        }

        [Test]
        public void TestTriggerNotExistsNullName()
        {
            Assert.Throws<ArgumentNullException>(() => TestTriggerExists(null));
        }

        [Test]
        public void TestTriggerNotExistsEmptyName()
        {
            Assert.Throws<ArgumentNullException>(() => TestTriggerExists(""));
        }

        [Test]
        public void TestTriggerExists()
        {
        	CreateTestTriggerInDatabase();

        	Assert.That(TestTriggerExists(TRIGGER_NAME), Is.True);
        }

    	protected abstract void CreateDatabaseTrigger(IDatabaseService connectedService, string triggerName);
        protected abstract void DropDatabaseTrigger(IDatabaseService connectedService, string triggerName);


		private void CreateTestTriggerInDatabase()
		{
			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				CreateDatabaseTrigger(connectedService, TRIGGER_NAME);
			}
		}

    	private bool TestTriggerExists(string trigger)
        {
            bool result;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.TriggerExists(trigger);
            }
            return result;
        }
		
		private void Cleanup()
		{
			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				DropDatabaseTrigger(connectedService, TRIGGER_NAME);
			}
		}
    }
}

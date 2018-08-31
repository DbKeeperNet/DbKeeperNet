using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Checkers
{
    public abstract class TriggerCheckerTestBase : TestBase
    {
        private const string UNKNOWN_TRIGGER_NAME = @"TR_some_unknown_testing";
        private const string TRIGGER_NAME = @"TR_some_testing";

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

        protected abstract void CreateDatabaseTrigger(string triggerName);
        protected abstract void DropDatabaseTrigger(string triggerName);


        private void CreateTestTriggerInDatabase()
        {
                CreateDatabaseTrigger(TRIGGER_NAME);
         
        }

        private bool TestTriggerExists(string trigger)
        {
            var checker = DefaultScope.ServiceProvider.GetService<IDatabaseServiceTriggerChecker>();

            var result = checker.Exists(trigger);

            return result;
        }

        private void Cleanup()
        {
                DropDatabaseTrigger(TRIGGER_NAME);
         
        }

    }
}
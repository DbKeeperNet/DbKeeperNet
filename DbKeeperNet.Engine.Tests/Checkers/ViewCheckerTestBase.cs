using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Checkers
{
    public abstract class ViewCheckerTestBase : TestBase
    {
        private const string VIEW_NAME = @"testing_view_name";
        private const string NONEXISTING_VIEW_NAME = @"unknown_testing_view_name";

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
        public void TestViewExists()
        {
            CreateTestViewInDatabase();

            Assert.That(TestViewExists(VIEW_NAME), Is.True);
        }

        [Test]
        public void TestViewNotExists()
        {
            CreateTestViewInDatabase();

            Assert.That(TestViewExists(NONEXISTING_VIEW_NAME), Is.False);
        }

        [Test]
        public void TestViewExistsNullName()
        {
            Assert.Throws<ArgumentNullException>(() => TestViewExists(null));
        }

        [Test]
        public void TestViewExistsEmptyName()
        {
            Assert.Throws<ArgumentNullException>(() => TestViewExists(String.Empty));
        }

        protected abstract void CreateView(string viewName);
        protected abstract void DropView(string viewName);

        private bool TestViewExists(string view)
        {
            var checker = DefaultScope.ServiceProvider.GetService<IDatabaseServiceViewChecker>();

            var result = checker.Exists(view);

            return result;
        }

        private void CreateTestViewInDatabase()
        {
            CreateView(VIEW_NAME);

        }

        private void Cleanup()
        {
            DropView(VIEW_NAME);

        }

    }
}
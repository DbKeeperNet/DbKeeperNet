using System;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceViewTests<T> : DatabaseServiceTests<T>
        where T : IDatabaseService, new()
    {
        private const string VIEW_NAME = @"testing_view_name";

        protected DatabaseServiceViewTests(string connectString)
            : base(connectString)
        {
        }

        protected abstract void CreateView(IDatabaseService connectedService, string viewName);
        protected abstract void DropView(IDatabaseService connectedService, string viewName);


        protected bool TestViewExists(string view)
        {
            bool result;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.ViewExists(view);
            }
            return result;
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
                DropView(connectedService, VIEW_NAME);
            }
        }
        [Test]
        public void TestViewExists()
        {
            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                CreateView(connectedService, VIEW_NAME);

                Assert.That(TestViewExists(VIEW_NAME), Is.True);
            }
        }

        [Test]
        public void TestViewNotExists()
        {
            Assert.That(TestViewExists(VIEW_NAME), Is.False);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewExistsNullName()
        {
            TestViewExists(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewExistsEmptyName()
        {
            TestViewExists(String.Empty);
        }
    }
}

using System;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceViewTests<T> : DatabaseServiceTests<T>
        where T : IDatabaseService, new()
    {
        private const string VIEW_NAME = @"testing_view_name";
		private const string NONEXISTING_VIEW_NAME = @"unknown_testing_view_name";

        protected DatabaseServiceViewTests(string connectString)
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

		protected abstract void CreateView(IDatabaseService connectedService, string viewName);
		protected abstract void DropView(IDatabaseService connectedService, string viewName);

		private bool TestViewExists(string view)
		{
			bool result;

			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				result = connectedService.ViewExists(view);
			}
			return result;
		}

		private void CreateTestViewInDatabase()
		{
			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				CreateView(connectedService, VIEW_NAME);
			}
		}

		private void Cleanup()
		{
			using (IDatabaseService connectedService = CreateConnectedDbService())
			{
				DropView(connectedService, VIEW_NAME);
			}
		}

    }
}

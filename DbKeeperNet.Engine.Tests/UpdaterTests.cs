using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using System.Reflection;

namespace DbKeeperNet.Engine.Tests
{
    [TestFixture]
    public class UpdaterTests
    {
        const string CONNECTION_STRING = "mock";
        const string LOGGER_NAME = "none";

        [Test]
        public void TestCustomStep()
        {
            MockRepository repository = new MockRepository();
            IDatabaseService driverMock = repository.StrictMock<IDatabaseService>();
            ILoggingService loggerMock = repository.Stub<ILoggingService>();

            using (repository.Record())
            {
                using (repository.Ordered())
                {
                    SetupResult.For(loggerMock.Name).Return(LOGGER_NAME);
                    SetupResult.For(driverMock.Name).Return("MockDriver");
                    SetupResult.For(driverMock.CloneForConnectionString(CONNECTION_STRING)).Return(driverMock);
                    SetupResult.For(driverMock.DatabaseSetupXml).Return(null);
                    Expect.Call(delegate { driverMock.BeginTransaction(); });
                    Expect.Call(delegate { driverMock.SetUpdateStepExecuted("DbUpdater.Engine", "1.00", 1); });
                    Expect.Call(delegate { driverMock.CommitTransaction(); });
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();

                context.RegisterLoggingService(loggerMock);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);

                Updater update = new Updater(context);
                update.ExecuteXml(Assembly.GetExecutingAssembly().GetManifestResourceStream("DbKeeperNet.Engine.Tests.CustomUpdateStep.xml"));
                Assert.That(CustomUpdateStep.Executed, Is.True);
            }

            repository.VerifyAll();
        }
    }
}

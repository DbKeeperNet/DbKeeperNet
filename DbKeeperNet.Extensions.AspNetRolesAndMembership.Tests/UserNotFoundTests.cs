using System.Reflection;
using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Tests;
using DbKeeperNet.Engine.Windows;
using DbKeeperNet.Extensions.AspNetRolesAndMembership.Preconditions;
using NUnit.Framework;
using Rhino.Mocks;
using Text = Rhino.Mocks.Constraints.Text;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership.Tests
{
    [TestFixture]
    public class UserNotFoundTests
    {
        const string CONNECTION_STRING = "mock";
        const string LOGGER_NAME = "none";

        [Test]
        public void TestUserNotFoundPreConditionTrueOnMockDriver()
        {
            MockRepository repository = new MockRepository();
            ILoggingService loggerStub = repository.Stub<ILoggingService>();
            IDatabaseService driverMock = repository.StrictMock<IDatabaseService>();
            IAspNetMembershipAdapter memberShipAdapterMock = repository.StrictMock<IAspNetMembershipAdapter>();

            using (repository.Record())
            {
                using (repository.Ordered())
                {
                    SetupResult.For(loggerStub.Name).Return(LOGGER_NAME);
                    SetupResult.For(driverMock.Name).Return("MockDriver");
                    SetupResult.For(driverMock.IsDbType("dbMock")).Return(true);
                    SetupResult.For(driverMock.CloneForConnectionString(CONNECTION_STRING)).Return(driverMock);
                    SetupResult.For(driverMock.DatabaseSetupXml).Return(null);

                    Expect.Call(memberShipAdapterMock.UserExists("SomeUser")).Return(false);
                    Expect.Call(driverMock.BeginTransaction);
                    Expect.Call(delegate { driverMock.ExecuteSql(null); }).Constraints(Text.Contains("query_to_be_executed_on_mock"));
                    Expect.Call(() => driverMock.SetUpdateStepExecuted("DbUpdater.Engine", "1.00", 1));
                    Expect.Call(driverMock.CommitTransaction);
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new WindowsUpdateContext();

                context.RegisterLoggingService(loggerStub);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);

                context.RegisterPrecondition(new UserNotFound(memberShipAdapterMock));
                context.RegisterUpdateStepHandler(new UpdateDbStepHandlerService(new NonSplittingSqlScriptSplitter()));

                Updater update = new Updater(context);
                update.ExecuteXml(Assembly.GetExecutingAssembly().GetManifestResourceStream("DbKeeperNet.Extensions.AspNetRolesAndMembership.Tests.UserNotFoundTests.xml"));
            }
            repository.VerifyAll();
        }
        [Test]
        public void TestUserNotFoundPreConditionFalseOnMockDriver()
        {
            MockRepository repository = new MockRepository();
            ILoggingService loggerStub = repository.Stub<ILoggingService>();
            IDatabaseService driverMock = repository.StrictMock<IDatabaseService>();
            IAspNetMembershipAdapter memberShipAdapterMock = repository.StrictMock<IAspNetMembershipAdapter>();

            using (repository.Record())
            {
                using (repository.Ordered())
                {
                    SetupResult.For(loggerStub.Name).Return(LOGGER_NAME);
                    SetupResult.For(driverMock.Name).Return("MockDriver");
                    SetupResult.For(driverMock.CloneForConnectionString(CONNECTION_STRING)).Return(driverMock);
                    SetupResult.For(driverMock.DatabaseSetupXml).Return(null);

                    Expect.Call(memberShipAdapterMock.UserExists("SomeUser")).Return(true);
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new WindowsUpdateContext();

                context.RegisterLoggingService(loggerStub);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);

                context.RegisterPrecondition(new UserNotFound(memberShipAdapterMock));
                context.RegisterUpdateStepHandler(new UpdateDbStepHandlerService(new NonSplittingSqlScriptSplitter()));

                Updater update = new Updater(context);
                update.ExecuteXml(Assembly.GetExecutingAssembly().GetManifestResourceStream("DbKeeperNet.Extensions.AspNetRolesAndMembership.Tests.UserNotFoundTests.xml"));
            }
            repository.VerifyAll();
        }
    }
}
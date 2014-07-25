using System.Reflection;
using DbKeeperNet.Engine.Extensions.Preconditions;
using NUnit.Framework;
using Rhino.Mocks;

namespace DbKeeperNet.Engine.Tests.Extensions.Preconditions
{
    [TestFixture]
    public class RoleNotFoundTests
    {
        const string CONNECTION_STRING = "mock";
        const string LOGGER_NAME = "none";

        [Test]
        public void TestRoleNotFoundPreConditionTrueOnMockDriver()
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

                    Expect.Call(memberShipAdapterMock.RoleExists("SomeRole")).Return(false);
                    Expect.Call(driverMock.BeginTransaction);
                    Expect.Call(() => driverMock.ExecuteSql("query_to_be_executed_on_mock"));
                    Expect.Call(() => driverMock.SetUpdateStepExecuted("DbUpdater.Engine", "1.00", 1));
                    Expect.Call(driverMock.CommitTransaction);
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();

                context.RegisterLoggingService(loggerStub);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);

                context.RegisterPrecondition(new RoleNotFound(memberShipAdapterMock));

                Updater update = new Updater(context, new UpdateStepVisitor(context, new NonSplittingSqlScriptSplitter(), new AspNetMembershipAdapter()));
                update.ExecuteXml(Assembly.GetExecutingAssembly().GetManifestResourceStream("DbKeeperNet.Engine.Tests.Extensions.Preconditions.RoleNotFoundTests.xml"));
            }
            repository.VerifyAll();
        }
        [Test]
        public void TestRoleNotFoundPreConditionFalseOnMockDriver()
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

                    Expect.Call(memberShipAdapterMock.RoleExists("SomeRole")).Return(true);
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();

                context.RegisterLoggingService(loggerStub);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);

                context.RegisterPrecondition(new RoleNotFound(memberShipAdapterMock));

                Updater update = new Updater(context, new UpdateStepVisitor(context, new NonSplittingSqlScriptSplitter(), new AspNetMembershipAdapter()));
                update.ExecuteXml(Assembly.GetExecutingAssembly().GetManifestResourceStream("DbKeeperNet.Engine.Tests.Extensions.Preconditions.RoleNotFoundTests.xml"));
            }
            repository.VerifyAll();
        }
    }
}
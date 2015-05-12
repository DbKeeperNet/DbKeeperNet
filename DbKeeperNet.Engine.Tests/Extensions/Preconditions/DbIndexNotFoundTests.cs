using NUnit.Framework;
using Rhino.Mocks;
using DbKeeperNet.Engine.Extensions.Preconditions;
using System.Reflection;
using Text = Rhino.Mocks.Constraints.Text;

namespace DbKeeperNet.Engine.Tests.Extensions.Preconditions
{
    [TestFixture]
    public class DbIndexNotFoundTests
    {
        const string CONNECTION_STRING = "mock";
        const string LOGGER_NAME = "none";

        [Test]
        public void TestDbIndexNotFoundPreConditionTrueOnMockDriver()
        {
            MockRepository repository = new MockRepository();
            ILoggingService loggerStub = repository.Stub<ILoggingService>();
            IDatabaseService driverMock = repository.StrictMock<IDatabaseService>();

            using (repository.Record())
            {
                using (repository.Ordered())
                {
                    SetupResult.For(loggerStub.Name).Return(LOGGER_NAME);
                    SetupResult.For(driverMock.Name).Return("MockDriver");
                    SetupResult.For(driverMock.IsDbType("dbMock")).Return(true);
                    SetupResult.For(driverMock.CloneForConnectionString(CONNECTION_STRING)).Return(driverMock);
                    SetupResult.For(driverMock.DatabaseSetupXml).Return(null);

                    Expect.Call(driverMock.IndexExists("test_index", "table_test_index")).Return(false);
                    Expect.Call(delegate { driverMock.BeginTransaction(); });
                    Expect.Call(delegate { driverMock.ExecuteSql(null); }).Constraints(Text.Contains("query_to_be_executed_on_mock"));
                    Expect.Call(delegate { driverMock.SetUpdateStepExecuted("DbUpdater.Engine", "1.00", 1); });
                    Expect.Call(delegate { driverMock.CommitTransaction(); });
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();

                context.RegisterLoggingService(loggerStub);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);

                context.RegisterPrecondition(new DbIndexNotFound());
                context.RegisterUpdateStepHandler(new UpdateDbStepHandlerService(new NonSplittingSqlScriptSplitter()));

                Updater update = new Updater(context);
                update.ExecuteXml(Assembly.GetExecutingAssembly().GetManifestResourceStream("DbKeeperNet.Engine.Tests.Extensions.Preconditions.DbIndexNotFoundTests.xml"));
            }
            repository.VerifyAll();
        }
        [Test]
        public void TestDbIndexNotFoundPreConditionFalseOnMockDriver()
        {
            MockRepository repository = new MockRepository();
            ILoggingService loggerStub = repository.Stub<ILoggingService>();
            IDatabaseService driverMock = repository.StrictMock<IDatabaseService>();

            using (repository.Record())
            {
                using (repository.Ordered())
                {
                    SetupResult.For(loggerStub.Name).Return(LOGGER_NAME);
                    SetupResult.For(driverMock.Name).Return("MockDriver");
                    SetupResult.For(driverMock.CloneForConnectionString(CONNECTION_STRING)).Return(driverMock);
                    SetupResult.For(driverMock.DatabaseSetupXml).Return(null);

                    Expect.Call(driverMock.IndexExists("test_index", "table_test_index")).Return(true);
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();

                context.RegisterLoggingService(loggerStub);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);

                context.RegisterPrecondition(new DbIndexNotFound());
                context.RegisterUpdateStepHandler(new UpdateDbStepHandlerService(new NonSplittingSqlScriptSplitter()));

                Updater update = new Updater(context);
                update.ExecuteXml(Assembly.GetExecutingAssembly().GetManifestResourceStream("DbKeeperNet.Engine.Tests.Extensions.Preconditions.DbIndexNotFoundTests.xml"));
            }
            repository.VerifyAll();
        }
    }
}

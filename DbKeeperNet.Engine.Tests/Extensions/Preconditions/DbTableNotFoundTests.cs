using NUnit.Framework;
using Rhino.Mocks;
using DbKeeperNet.Engine.Extensions.Preconditions;
using System.Reflection;

namespace DbKeeperNet.Engine.Tests.Extensions.Preconditions
{
    [TestFixture]
    public class DbTableNotFoundTests
    {
        const string CONNECTION_STRING = "mock";
        const string LOGGER_NAME = "none";

        [Test]
        public void TestDbTableNotFoundPreConditionTrueOnMockDriver()
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

                    Expect.Call(driverMock.TableExists("test_table")).Return(false);
                    Expect.Call(delegate { driverMock.BeginTransaction(); });
                    Expect.Call(delegate { driverMock.ExecuteSql("query_to_be_executed_on_mock"); });
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

                context.RegisterPrecondition(new DbTableNotFound());

                Updater update = new Updater(context, new UpdateStepVisitor(context, new NonSplittingSqlScriptSplitter()));
                update.ExecuteXml(Assembly.GetExecutingAssembly().GetManifestResourceStream("DbKeeperNet.Engine.Tests.Extensions.Preconditions.DbTableNotFoundTests.xml"));
            }
            repository.VerifyAll();
        }
        [Test]
        public void TestDbTableNotFoundPreConditionFalseOnMockDriver()
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

                    Expect.Call(driverMock.TableExists("test_table")).Return(true);
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();

                context.RegisterLoggingService(loggerStub);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);

                context.RegisterPrecondition(new DbTableNotFound());

                Updater update = new Updater(context, new UpdateStepVisitor(context, new NonSplittingSqlScriptSplitter()));
                update.ExecuteXml(Assembly.GetExecutingAssembly().GetManifestResourceStream("DbKeeperNet.Engine.Tests.Extensions.Preconditions.DbTableNotFoundTests.xml"));
            }
            repository.VerifyAll();
        }
    }
}

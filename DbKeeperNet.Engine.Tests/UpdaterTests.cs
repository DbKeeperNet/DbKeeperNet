using NUnit.Framework;
using Rhino.Mocks;
using System.Reflection;
using System.Configuration;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Extensions.ScriptProviderServices;
using DbKeeperNet.Engine.Tests.Extensions.Preconditions;

namespace DbKeeperNet.Engine.Tests
{
    [TestFixture]
    public class UpdaterTests
    {
        private const string CONNECTION_STRING = "mock";
        private const string LOGGER_NAME = "none";

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
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());

                context.RegisterLoggingService(loggerMock);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);
                context.LoadExtensions();

                Updater update = new Updater(context);
                update.ExecuteXml(
                    Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("DbKeeperNet.Engine.Tests.CustomUpdateStep.xml"));
                Assert.That(CustomUpdateStep.Executed, Is.True);
            }

            repository.VerifyAll();
        }
        
        [Test]
        public void MultipleConditionsEvaluatingToTrueShouldRunStep()
        {
            MockRepository repository = new MockRepository();
            IDatabaseService driverMock = repository.StrictMock<IDatabaseService>();
            ILoggingService loggerMock = repository.Stub<ILoggingService>();
            IPrecondition precondition1 = repository.StrictMock<IPrecondition>();
            IPrecondition precondition2 = repository.StrictMock<IPrecondition>();

            using (repository.Record())
            {
                using (repository.Ordered())
                {
                    SetupResult.For(loggerMock.Name).Return(LOGGER_NAME);
                    SetupResult.For(driverMock.Name).Return("MockDriver");
                    SetupResult.For(driverMock.IsDbType("dbMock")).Return(true);
                    SetupResult.For(driverMock.CloneForConnectionString(CONNECTION_STRING)).Return(driverMock);
                    SetupResult.For(driverMock.DatabaseSetupXml).Return(null);

                    SetupResult.For(precondition1.Name).Return("precondition1");
                    SetupResult.For(precondition2.Name).Return("precondition2");

                    Expect.Call(delegate { driverMock.BeginTransaction(); });
                    
                    SetupResult.For(precondition1.CheckPrecondition(Arg<IUpdateContext>.Is.Anything,
                        Arg<PreconditionParamType[]>.Is.Anything)).Return(true);
                    SetupResult.For(precondition2.CheckPrecondition(Arg<IUpdateContext>.Is.Anything,
                        Arg<PreconditionParamType[]>.Is.Anything)).Return(true);
                    
                    Expect.Call(delegate { driverMock.ExecuteSql(null); }).Constraints(Rhino.Mocks.Constraints.Text.Contains("query_to_be_executed_on_mock"));
                    Expect.Call(() => driverMock.SetUpdateStepExecuted("DbUpdater.Engine", "1.00", 1));
                    Expect.Call(delegate { driverMock.CommitTransaction(); });
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());

                context.RegisterLoggingService(loggerMock);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);
                
                context.RegisterPrecondition(precondition1);
                context.RegisterPrecondition(precondition2);
                context.RegisterUpdateStepHandler(new UpdateDbStepHandlerService(new NonSplittingSqlScriptSplitter()));

                Updater update = new Updater(context);
                update.ExecuteXml(
                    Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("DbKeeperNet.Engine.Tests.MultiConditions.xml"));
            }

            repository.VerifyAll();
        }        
        
        [Test]
        public void MultipleConditionsWhenFirstEvaluatingToTrueAndSecondToFalseShouldNotRunStep()
        {
            MockRepository repository = new MockRepository();
            IDatabaseService driverMock = repository.StrictMock<IDatabaseService>();
            ILoggingService loggerMock = repository.Stub<ILoggingService>();
            IPrecondition precondition1 = repository.StrictMock<IPrecondition>();
            IPrecondition precondition2 = repository.StrictMock<IPrecondition>();

            using (repository.Record())
            {
                using (repository.Ordered())
                {
                    SetupResult.For(loggerMock.Name).Return(LOGGER_NAME);
                    SetupResult.For(driverMock.Name).Return("MockDriver");
                    SetupResult.For(driverMock.IsDbType("dbMock")).Return(true);
                    SetupResult.For(driverMock.CloneForConnectionString(CONNECTION_STRING)).Return(driverMock);
                    SetupResult.For(driverMock.DatabaseSetupXml).Return(null);

                    SetupResult.For(precondition1.Name).Return("precondition1");
                    SetupResult.For(precondition2.Name).Return("precondition2");

                    SetupResult.For(precondition1.CheckPrecondition(Arg<IUpdateContext>.Is.Anything,
                        Arg<PreconditionParamType[]>.Is.Anything)).Return(true);
                    SetupResult.For(precondition2.CheckPrecondition(Arg<IUpdateContext>.Is.Anything,
                        Arg<PreconditionParamType[]>.Is.Anything)).Return(false);
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());

                context.RegisterLoggingService(loggerMock);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);
                
                context.RegisterPrecondition(precondition1);
                context.RegisterPrecondition(precondition2);
                context.RegisterUpdateStepHandler(new UpdateDbStepHandlerService(new NonSplittingSqlScriptSplitter()));

                Updater update = new Updater(context);
                update.ExecuteXml(
                    Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("DbKeeperNet.Engine.Tests.MultiConditions.xml"));
            }

            repository.VerifyAll();
        }

        [Test]
        public void MultipleConditionsWhenFirstEvaluatingToFalseAndSecondIsNotEvaluatedShouldNotRunStep()
        {
            MockRepository repository = new MockRepository();
            IDatabaseService driverMock = repository.StrictMock<IDatabaseService>();
            ILoggingService loggerMock = repository.Stub<ILoggingService>();
            IPrecondition precondition1 = repository.StrictMock<IPrecondition>();
            IPrecondition precondition2 = repository.StrictMock<IPrecondition>();

            using (repository.Record())
            {
                using (repository.Ordered())
                {
                    SetupResult.For(loggerMock.Name).Return(LOGGER_NAME);
                    SetupResult.For(driverMock.Name).Return("MockDriver");
                    SetupResult.For(driverMock.IsDbType("dbMock")).Return(true);
                    SetupResult.For(driverMock.CloneForConnectionString(CONNECTION_STRING)).Return(driverMock);
                    SetupResult.For(driverMock.DatabaseSetupXml).Return(null);

                    SetupResult.For(precondition1.Name).Return("precondition1");
                    SetupResult.For(precondition2.Name).Return("precondition2");

                    SetupResult.For(precondition1.CheckPrecondition(Arg<IUpdateContext>.Is.Anything,
                        Arg<PreconditionParamType[]>.Is.Anything)).Return(false);
                }
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());

                context.RegisterLoggingService(loggerMock);
                context.InitializeLoggingService(LOGGER_NAME);

                context.RegisterDatabaseService(driverMock);
                context.InitializeDatabaseService(CONNECTION_STRING);
                
                context.RegisterPrecondition(precondition1);
                context.RegisterPrecondition(precondition2);
                context.RegisterUpdateStepHandler(new UpdateDbStepHandlerService(new NonSplittingSqlScriptSplitter()));

                Updater update = new Updater(context);
                update.ExecuteXml(
                    Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("DbKeeperNet.Engine.Tests.MultiConditions.xml"));
            }

            repository.VerifyAll();
        }
    }
}

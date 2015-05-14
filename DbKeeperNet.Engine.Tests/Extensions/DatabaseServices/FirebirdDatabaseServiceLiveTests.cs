using DbKeeperNet.Engine.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    /// <summary>
    /// Tests which requires configured Firebird database.
    /// As prerequisities may be those table created.
    /// </summary>
    [TestFixture]
    [Explicit]
    [Category("firebird")]
    public class FirebirdDatabaseServiceLiveTests: DatabaseServiceTests<FirebirdDatabaseService>
    {
        const string CONNECTION_STRING = @"firebird";

        public FirebirdDatabaseServiceLiveTests() : base(CONNECTION_STRING)
        {
        }

        [SetUp]
        public void Setup()
        {
            Cleanup();

            IUpdateContext context = new UpdateContext();
            context.LoadExtensions();
            context.InitializeDatabaseService(ConnectionString);

            Updater updater = new Updater(context);
            updater.ExecuteXml(typeof(DbServicesExtension).Assembly.GetManifestResourceStream(@"DbKeeperNet.Engine.Extensions.DatabaseServices.FirebirdDatabaseServiceInstall.xml"));
        }

        [TearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void TestIsUpdateStepExecutedFalse()
        {
            FirebirdDatabaseService service = new FirebirdDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(ConnectionString))
            {
                Assert.That(connectedService.IsUpdateStepExecuted("MyTestingAssembly", "x.x99", 222), Is.False);
            }
        }

        [Test]
        public void TestSetUpdateStepExecuted()
        {
            FirebirdDatabaseService service = new FirebirdDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(ConnectionString))
            {
                connectedService.SetUpdateStepExecuted("MyTestingAssembly.TestSetUpdateStepExecuted", "1.00", 1);
                Assert.That(connectedService.IsUpdateStepExecuted("MyTestingAssembly.TestSetUpdateStepExecuted", "1.00", 1), Is.True);
            }
        }

        private void Cleanup()
        {
            using (var service = CreateConnectedDbService())
            {
                ExecuteSqlAndIgnoreException(service, @"DROP TRIGGER ""TR_dbkeepernet_step""");
                ExecuteSqlAndIgnoreException(service, @"DROP TRIGGER ""TR_dbkeepernet_version""");
                ExecuteSqlAndIgnoreException(service, @"DROP TRIGGER ""TR_dbkeepernet_assembly""");

                ExecuteSqlAndIgnoreException(service, @"DROP GENERATOR ""GEN_dbkeepernet_step_id""");
                ExecuteSqlAndIgnoreException(service, @"DROP GENERATOR ""GEN_dbkeepernet_version_id""");
                ExecuteSqlAndIgnoreException(service, @"DROP GENERATOR ""GEN_dbkeepernet_assembly_id""");


                ExecuteSqlAndIgnoreException(service, @"DROP TABLE ""dbkeepernet_step""");
                ExecuteSqlAndIgnoreException(service, @"DROP TABLE ""dbkeepernet_version""");
                ExecuteSqlAndIgnoreException(service, @"DROP TABLE ""dbkeepernet_assembly""");
            }
        }
    }
}
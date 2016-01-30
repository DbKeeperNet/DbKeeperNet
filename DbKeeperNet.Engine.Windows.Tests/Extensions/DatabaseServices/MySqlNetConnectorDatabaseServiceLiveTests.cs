using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("mysql")]
    public class MySqlNetConnectorDatabaseServiceLiveTests : DatabaseServiceTests<MySqlNetConnectorDatabaseService>
    {
        const string CONNECTION_STRING = "mysql";

        public MySqlNetConnectorDatabaseServiceLiveTests() : base(CONNECTION_STRING)
        {
        }

        [SetUp]
        public void Setup()
        {
            Cleanup();

            IUpdateContext context = new WindowsUpdateContext();
            context.LoadExtensions();
            context.InitializeDatabaseService(ConnectionString);

            Updater updater = new Updater(context);
            updater.ExecuteXml(typeof(DbServicesExtension).Assembly.GetManifestResourceStream("DbKeeperNet.Engine.Windows.Extensions.DatabaseServices.MySqlNetConnectorDatabaseServiceInstall.xml"));
        }

        [TearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void TestIsUpdateStepExecutedFalse()
        {
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(ConnectionString))
            {
                Assert.That(connectedService.IsUpdateStepExecuted("MyTestingAssembly", "x.x99", 222), Is.False);
            }
        }

        [Test]
        public void TestSetUpdateStepExecuted()
        {
            MySqlNetConnectorDatabaseService service = new MySqlNetConnectorDatabaseService();

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
                ExecuteSqlAndIgnoreException(service, @"DROP TABLE dbkeepernet_step");
                ExecuteSqlAndIgnoreException(service, @"DROP TABLE dbkeepernet_version");
                ExecuteSqlAndIgnoreException(service, @"DROP TABLE dbkeepernet_assembly");
            }
        }
    }
}

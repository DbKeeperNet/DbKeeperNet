using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    /// <summary>
    /// Tests which requires configured MSSQL database.
    /// As prerequisities may be those table created.
    /// </summary>
    [TestFixture]
    [Explicit]
    [Category("oracle")]
    public class OracleDatabaseServicesLiveTests : DatabaseServiceTests<OracleDatabaseService>
    {
        const string CONNECTION_STRING = "oracle";

        public OracleDatabaseServicesLiveTests() : base(CONNECTION_STRING)
        {
        }

        [SetUp]
        public void Setup()
        {
            Cleanup();

            IUpdateContext context = new UpdateContext();
            context.LoadExtensions();
            context.InitializeDatabaseService(CONNECTION_STRING);

            Updater updater = new Updater(context);
            updater.ExecuteXml(typeof(DbServicesExtension).Assembly.GetManifestResourceStream("DbKeeperNet.Engine.Extensions.DatabaseServices.OracleDatabaseServiceInstall.xml"));
        }

        [TearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void TestIsUpdateStepExecutedFalse()
        {
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(ConnectionString))
            {
                Assert.That(connectedService.IsUpdateStepExecuted("MyTestingAssembly", "x.x99", 222), Is.False);
            }
        }

        [Test]
        public void TestSetUpdateStepExecuted()
        {
            OracleDatabaseService service = new OracleDatabaseService();

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
                ExecuteSqlAndIgnoreException(service, @"DROP TRIGGER ""BI_DBKEEPERNET_STEP""");
                ExecuteSqlAndIgnoreException(service, @"DROP TRIGGER ""BI_DBKEEPERNET_VERSION""");
                ExecuteSqlAndIgnoreException(service, @"DROP TRIGGER ""BI_DBKEEPERNET_ASSEMBLY""");

                ExecuteSqlAndIgnoreException(service, @"DROP GENERATOR ""DBKEEPERNET_STEP_SEQ""");
                ExecuteSqlAndIgnoreException(service, @"DROP GENERATOR ""DBKEEPERNET_VERSION_SEQ""");
                ExecuteSqlAndIgnoreException(service, @"DROP GENERATOR ""DBKEEPERNET_ASSEMBLY_SEQ""");


                ExecuteSqlAndIgnoreException(service, @"DROP TABLE ""DBKEEPERNET_STEP""");
                ExecuteSqlAndIgnoreException(service, @"DROP TABLE ""DBKEEPERNET_VERSION""");
                ExecuteSqlAndIgnoreException(service, @"DROP TABLE ""DBKEEPERNET_ASSEMBLY""");
            }
        }
    }
}

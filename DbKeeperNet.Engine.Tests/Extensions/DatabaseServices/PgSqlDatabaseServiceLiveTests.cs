using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("pgsql")]
    public class PgSqlDatabaseServiceLiveTests : DatabaseServiceTests<PgSqlDatabaseService>
    {
        const string CONNECTION_STRING = "pgsql";

        public PgSqlDatabaseServiceLiveTests() : base(CONNECTION_STRING)
        {
        }

        [TearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [SetUp]
        public void Setup()
        {
            Cleanup();

            IUpdateContext context = new UpdateContext();
            context.LoadExtensions();
            context.InitializeDatabaseService(ConnectionString);

            Updater updater = new Updater(context);
            updater.ExecuteXml(typeof(DbServicesExtension).Assembly.GetManifestResourceStream("DbKeeperNet.Engine.Extensions.DatabaseServices.PgSqlDatabaseServiceInstall.xml"));
        }

        [Test]
        public void TestIsUpdateStepExecutedFalse()
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(ConnectionString))
            {
                Assert.That(connectedService.IsUpdateStepExecuted("MyTestingAssembly", "x.x99", 222), Is.False);
            }
        }

        [Test]
        public void TestSetUpdateStepExecuted()
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();

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

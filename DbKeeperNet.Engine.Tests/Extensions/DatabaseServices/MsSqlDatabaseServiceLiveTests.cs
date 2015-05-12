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
    [Category("mssql")]
    public class MsSqlDatabaseServicesLiveTests : DatabaseServiceTests<MsSqlDatabaseService>
    {
        const string CONNECTION_STRING = "mssql";

        public MsSqlDatabaseServicesLiveTests() : base(CONNECTION_STRING)
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
            updater.ExecuteXml(typeof(DbServicesExtension).Assembly.GetManifestResourceStream("DbKeeperNet.Engine.Extensions.DatabaseServices.MsSqlDatabaseServiceInstall.xml"));
        }


        [TearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void TestIsUpdateStepExecutedFalse()
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(ConnectionString))
            {
                Assert.That(connectedService.IsUpdateStepExecuted("MyTestingAssembly", "x.x99", 222), Is.False);
            }
        }

        [Test]
        public void TestSetUpdateStepExecuted()
        {
            MsSqlDatabaseService service = new MsSqlDatabaseService();

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
                ExecuteSqlAndIgnoreException(service, @"DROP PROCEDURE DbKeeperNetSetStepExecuted");
                ExecuteSqlAndIgnoreException(service, @"DROP PROCEDURE DbKeeperNetIsStepExecuted");

                ExecuteSqlAndIgnoreException(service, @"DROP TABLE dbkeepernet_step");
                ExecuteSqlAndIgnoreException(service, @"DROP TABLE dbkeepernet_version");
                ExecuteSqlAndIgnoreException(service, @"DROP TABLE dbkeepernet_assembly");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DbKeeperNet.Engine.Extensions.DatabaseServices;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    [TestFixture]
    [Explicit]
    [Category("pgsql")]
    public class PgSqlDatabaseServiceLiveTests
    {
        const string CONNECTION_STRING = "pgsql";

        [SetUp]
        public void Setup()
        {
            IUpdateContext context = new UpdateContext();
            context.LoadExtensions();
            context.InitializeDatabaseService(CONNECTION_STRING);

            Updater updater = new Updater(context);
            updater.ExecuteXml(typeof(DbServicesExtension).Assembly.GetManifestResourceStream("DbKeeperNet.Engine.Extensions.DatabaseServices.PgSqlDatabaseServiceInstall.xml"));
        }
        [Test]
        public void TestIsUpdateStepExecutedFalse()
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                Assert.That(connectedService.IsUpdateStepExecuted("MyTestingAssembly", "x.x99", 222), Is.False);
            }
        }
        [Test]
        public void TestSetUpdateStepExecuted()
        {
            PgSqlDatabaseService service = new PgSqlDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                connectedService.SetUpdateStepExecuted("MyTestingAssembly.TestSetUpdateStepExecuted", "1.00", 1);
                Assert.That(connectedService.IsUpdateStepExecuted("MyTestingAssembly.TestSetUpdateStepExecuted", "1.00", 1), Is.True);
            }
        }
    }
}

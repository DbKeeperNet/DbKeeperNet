using System;
using System.Collections.Generic;
using System.Text;
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
    public class OracleDatabaseServicesLiveTests
    {
        const string CONNECTION_STRING = "oracle";

        [SetUp]
        public void Setup()
        {
            IUpdateContext context = new UpdateContext();
            context.LoadExtensions();
            context.InitializeDatabaseService(CONNECTION_STRING);

            Updater updater = new Updater(context);
            updater.ExecuteXml(typeof(DbServicesExtension).Assembly.GetManifestResourceStream("DbKeeperNet.Engine.Extensions.DatabaseServices.OracleDatabaseServiceInstall.xml"));
        }
        [Test]
        public void TestIsUpdateStepExecutedFalse()
        {
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                Assert.That(connectedService.IsUpdateStepExecuted("MyTestingAssembly", "x.x99", 222), Is.False);
            }
        }
        [Test]
        public void TestSetUpdateStepExecuted()
        {
            OracleDatabaseService service = new OracleDatabaseService();

            using (IDatabaseService connectedService = service.CloneForConnectionString(CONNECTION_STRING))
            {
                connectedService.SetUpdateStepExecuted("MyTestingAssembly.TestSetUpdateStepExecuted", "1.00", 1);
                Assert.That(connectedService.IsUpdateStepExecuted("MyTestingAssembly.TestSetUpdateStepExecuted", "1.00", 1), Is.True);
            }
        }
    }
}

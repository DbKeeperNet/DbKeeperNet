using System;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests
{
    public abstract class EndToEndTestBase : TestBase
    {
        [Test]
        public void TestEndToEndSetup()
        {
            using (var s = ServiceProvider.CreateScope())
            {
                var updater = s.ServiceProvider.GetService<IDatabaseUpdater>();
                updater.ExecuteUpgrade();
                updater.ExecuteUpgrade();

                AssertThatTableExists(s.ServiceProvider.GetService<IDatabaseService>());
            }
        }

        [Test]
        public void TestEndToEndSetupThruLazyResolution()
        {
            using (var s = ServiceProvider.CreateScope())
            {
                var updater = s.ServiceProvider.GetService<Lazy<IDatabaseUpdater>>();
                updater.Value.ExecuteUpgrade();
                updater.Value.ExecuteUpgrade();

                AssertThatTableExists(s.ServiceProvider.GetService<IDatabaseService>());
            }
        }
        
        protected virtual void AssertThatTableExists(IDatabaseService connection)
        {
            var commandText = "select * from DbKeeperNet_SimpleDemo";

            ExecuteCommand(connection, commandText);
        }

        protected static void ExecuteCommand(IDatabaseService connection, string commandText)
        {
            var databaseConnection = connection.GetOpenConnection();
            using (var cmd = databaseConnection.CreateCommand())
            {
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
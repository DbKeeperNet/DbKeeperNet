using DbKeeperNet.Engine;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SqlServer.Tests
{
    [TestFixture]
    public class MicrosoftSqlDatabaseServiceInstallerTest
    {
        [Test]
        public void GetInstallerScriptShouldReturnStream()
        {
            var installer = new MicrosoftSqlDatabaseServiceInstaller();

            Assert.That(installer.GetInstallerScript(), Is.Not.Null);
        }
    }
}
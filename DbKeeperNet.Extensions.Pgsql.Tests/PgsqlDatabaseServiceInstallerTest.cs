using NUnit.Framework;

namespace DbKeeperNet.Extensions.Pgsql.Tests
{
    [TestFixture]
    public class PgsqlDatabaseServiceInstallerTest
    {
        [Test]
        public void GetInstallerScriptShouldReturnStream()
        {
            var installer = new PgsqlDatabaseServiceInstaller();

            Assert.That(installer.GetInstallerScript(), Is.Not.Null);
        }
    }
}
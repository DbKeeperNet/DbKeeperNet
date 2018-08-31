using NUnit.Framework;

namespace DbKeeperNet.Extensions.SQLite.Tests
{
    [TestFixture]
    public class SQLiteDatabaseServiceInstallerTest
    {
        [Test]
        public void GetInstallerScriptShouldReturnStream()
        {
            var installer = new SQLiteDatabaseServiceInstaller();
            
            Assert.That(installer.GetInstallerScript(), Is.Not.Null);
        }
    }
}

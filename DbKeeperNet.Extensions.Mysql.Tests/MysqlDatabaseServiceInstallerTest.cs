using NUnit.Framework;

namespace DbKeeperNet.Extensions.Mysql.Tests
{
    [TestFixture]
    public class MysqlDatabaseServiceInstallerTest
    {
        [Test]
        public void GetInstallerScriptShouldReturnStream()
        {
            var installer = new MysqlDatabaseServiceInstaller();

            Assert.That(installer.GetInstallerScript(), Is.Not.Null);
        }
    }
}
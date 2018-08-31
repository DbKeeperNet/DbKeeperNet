using NUnit.Framework;

namespace DbKeeperNet.Extensions.Firebird.Tests
{
    [TestFixture]
    public class FirebirdDatabaseServiceInstallerTest
    {
        [Test]
        public void GetInstallerScriptShouldReturnStream()
        {
            var installer = new FirebirdDatabaseServiceInstaller();

            Assert.That(installer.GetInstallerScript(), Is.Not.Null);
        }
    }
}
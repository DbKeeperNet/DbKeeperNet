using System.IO;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Firebird
{
    public class FirebirdDatabaseServiceInstaller : IDatabaseServiceInstaller
    {
        public Stream GetInstallerScript()
        {
            return typeof(FirebirdDatabaseServiceInstaller).Assembly.GetManifestResourceStream(
                "DbKeeperNet.Extensions.Firebird.FirebirdDatabaseServiceInstall.xml");
        }
    }
}
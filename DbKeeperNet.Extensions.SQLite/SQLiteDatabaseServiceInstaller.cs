using System.IO;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.SQLite
{
    public class SQLiteDatabaseServiceInstaller : IDatabaseServiceInstaller
    {
        public Stream GetInstallerScript()
        {
            return typeof(SQLiteDatabaseServiceInstaller).Assembly.GetManifestResourceStream(
                "DbKeeperNet.Extensions.SQLite.SQLiteDatabaseServiceInstall.xml");
        }
    }
}
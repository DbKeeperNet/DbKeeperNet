using System.IO;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.SqlServer
{
    public class MicrosoftSqlDatabaseServiceInstaller : IDatabaseServiceInstaller
    {
        public Stream GetInstallerScript()
        {
            return typeof(MicrosoftSqlDatabaseServiceInstaller).Assembly.GetManifestResourceStream(
                "DbKeeperNet.Extensions.SqlServer.MsSqlDatabaseServiceInstall.xml");
        }
    }
}
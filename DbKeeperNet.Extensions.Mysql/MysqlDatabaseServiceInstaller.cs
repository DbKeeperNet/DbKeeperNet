using System.IO;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Mysql
{
    public class MysqlDatabaseServiceInstaller : IDatabaseServiceInstaller
    {
        public Stream GetInstallerScript()
        {
            return typeof(MysqlDatabaseServiceInstaller).Assembly.GetManifestResourceStream(
                "DbKeeperNet.Extensions.Mysql.MysqlDatabaseServiceInstall.xml");
        }
    }
}

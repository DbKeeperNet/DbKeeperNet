using System.IO;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Pgsql
{
    public class PgsqlDatabaseServiceInstaller : IDatabaseServiceInstaller
    {
        public Stream GetInstallerScript()
        {
            return typeof(PgsqlDatabaseServiceInstaller).Assembly.GetManifestResourceStream(
                "DbKeeperNet.Extensions.Pgsql.PgSqlDatabaseServiceInstall.xml");
        }
    }
}
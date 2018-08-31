using System.IO;

namespace DbKeeperNet.Engine
{
    public interface IDatabaseServiceInstaller
    {
        Stream GetInstallerScript();
    }
}
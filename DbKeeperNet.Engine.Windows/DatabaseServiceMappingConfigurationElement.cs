using System.Configuration;

namespace DbKeeperNet.Engine.Windows
{
    public class DatabaseServiceMappingConfigurationElement : ConfigurationElement, IDatabaseServiceMappingConfigurationElement
    {
        [ConfigurationProperty("connectString", IsRequired = true)]
        public string ConnectString
        {
            get { return (string)this["connectString"]; }
        }
        [ConfigurationProperty("databaseService", IsRequired = true)]
        public string DatabaseService
        {
            get
            {
                return (string)this["databaseService"];
            }
        }
    }
}

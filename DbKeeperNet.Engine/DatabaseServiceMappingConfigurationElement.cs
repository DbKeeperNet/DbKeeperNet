using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DbKeeperNet.Engine
{
    public class DatabaseServiceMappingConfigurationElement : ConfigurationElement
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

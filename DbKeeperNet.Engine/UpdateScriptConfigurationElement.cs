using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DbKeeperNet.Engine
{
    public class UpdateScriptConfigurationElement: ConfigurationElement
    {
        [ConfigurationProperty("location", IsRequired = true, IsKey = true)]
        public string Location
        {
            get { return (string)this["location"]; }
            set { this["location"] = value; }
        }
        [ConfigurationProperty("provider", IsRequired = true)]
        public string Provider
        {
            get { return (string)this["provider"]; }
            set { this["provider"] = value; }
        }
    }
}

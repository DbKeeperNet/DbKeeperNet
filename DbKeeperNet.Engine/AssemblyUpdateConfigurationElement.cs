using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DbKeeperNet.Engine
{
    public sealed class AssemblyUpdateConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get { return (string)this["assembly"]; }
            set { this["assembly"] = value; }
        }
        [ConfigurationProperty("manifestResource", IsRequired = true, IsKey = true)]
        public string ManifestResource
        {
            get { return (string)this["manifestResource"]; }
            set { this["manifestResource"] = value; }
        }
    }
}

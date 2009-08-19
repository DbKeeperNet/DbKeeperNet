using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DbKeeperNet.Engine
{
    public sealed class AssemblyUpdateConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("assemblyPath", IsRequired = true)]
        public string AssemblyPath
        {
            get { return (string)this["assemblyPath"]; }
            set { this["assemblyPath"] = value; }
        }
        [ConfigurationProperty("manifestResource", IsRequired = true, IsKey = true)]
        public string ManifestResource
        {
            get { return (string)this["manifestResource"]; }
            set { this["manifestResource"] = value; }
        }
    }
}

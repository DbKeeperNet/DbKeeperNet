using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DbKeeperNet.Engine
{
    public sealed class ExtensionConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("assemblyPath", IsKey = true, IsRequired = true)]
        public string AssemblyPath
        {
            get { return (string)this["assemblyPath"]; }
            set { this["assemblyPath"] = value; }
        }
    }
}

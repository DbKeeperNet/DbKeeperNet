using System.Configuration;

namespace DbKeeperNet.Engine
{
    public sealed class ExtensionConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("assembly", IsKey = true, IsRequired = true)]
        public string Assembly
        {
            get { return (string)this["assembly"]; }
            set { this["assembly"] = value; }
        }
    }
}

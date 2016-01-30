using System.Configuration;

namespace DbKeeperNet.Engine.Windows
{
    public sealed class ExtensionConfigurationElement : ConfigurationElement, IExtensionConfigurationElement
    {
        [ConfigurationProperty("assembly", IsKey = true, IsRequired = true)]
        public string Assembly
        {
            get { return (string)this["assembly"]; }
            set { this["assembly"] = value; }
        }
    }
}

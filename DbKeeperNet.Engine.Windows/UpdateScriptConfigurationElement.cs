using System.Configuration;

namespace DbKeeperNet.Engine.Windows
{
    public class UpdateScriptConfigurationElement: ConfigurationElement, IUpdateScriptConfigurationElement
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

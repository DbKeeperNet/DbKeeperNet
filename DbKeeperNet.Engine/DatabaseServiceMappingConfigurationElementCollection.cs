using System.Configuration;

namespace DbKeeperNet.Engine
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public class DatabaseServiceMappingConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DatabaseServiceMappingConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DatabaseServiceMappingConfigurationElement)element).ConnectString;
        }

        public DatabaseServiceMappingConfigurationElement this[int index]
        {
            get
            {
                return (DatabaseServiceMappingConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }
    }
}

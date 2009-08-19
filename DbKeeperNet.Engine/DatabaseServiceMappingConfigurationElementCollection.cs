using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DbKeeperNet.Engine
{
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

        public DatabaseServiceMappingConfigurationElement this[int i]
        {
            get
            {
                return (DatabaseServiceMappingConfigurationElement)BaseGet(i);
            }
            set
            {
                if (BaseGet(i) != null)
                    BaseRemoveAt(i);

                BaseAdd(i, value);
            }
        }
    }
}

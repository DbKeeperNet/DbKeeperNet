using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DbKeeperNet.Engine
{
    public class UpdateScriptConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UpdateScriptConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UpdateScriptConfigurationElement)element).Location;
        }

        public UpdateScriptConfigurationElement this[int i]
        {
            get
            {
                return (UpdateScriptConfigurationElement)BaseGet(i);
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

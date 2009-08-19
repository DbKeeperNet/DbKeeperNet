using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DbKeeperNet.Engine
{
    public sealed class AssemblyUpdateConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyUpdateConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyUpdateConfigurationElement)element).ManifestResource;
        }
        public AssemblyUpdateConfigurationElement this[int i]
        {
            get
            {
                return (AssemblyUpdateConfigurationElement)BaseGet(i);
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DbKeeperNet.Engine
{
    public sealed class ExtensionConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExtensionConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExtensionConfigurationElement)element).AssemblyPath;
        }
        public ExtensionConfigurationElement this[int i]
        {
            get
            {
                return (ExtensionConfigurationElement)BaseGet(i);
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

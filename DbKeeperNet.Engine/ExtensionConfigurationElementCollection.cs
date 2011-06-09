using System.Configuration;
using System.Reflection;

namespace DbKeeperNet.Engine
{
    public sealed class ExtensionConfigurationElementCollection : ConfigurationElementCollection
    {
        public ExtensionConfigurationElementCollection()
        {
            ExtensionConfigurationElement e = new ExtensionConfigurationElement();
            e.Assembly = Assembly.GetExecutingAssembly().FullName;

            BaseAdd(e);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ExtensionConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExtensionConfigurationElement)element).Assembly;
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

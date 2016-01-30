using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace DbKeeperNet.Engine.Windows
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public sealed class ExtensionConfigurationElementCollection : ConfigurationElementCollection, IExtensionConfigurationElementCollection
    {
        public ExtensionConfigurationElementCollection()
        {
            var e = new ExtensionConfigurationElement
            {
                Assembly = typeof (ExtensionConfigurationElementCollection).GetTypeInfo().Assembly.FullName
            };
            BaseAdd(e);

            e = new ExtensionConfigurationElement
            {
                Assembly = typeof (IExtensionConfigurationElementCollection).GetTypeInfo().Assembly.FullName
            };

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
        public ExtensionConfigurationElement this[int index]
        {
            get
            {
                return (ExtensionConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        IEnumerator<IExtensionConfigurationElement> IEnumerable<IExtensionConfigurationElement>.GetEnumerator()
        {
            foreach (IExtensionConfigurationElement e in this)
            {
                yield return e;
            }
        }

        IExtensionConfigurationElement IExtensionConfigurationElementCollection.this[int index]
        {
            get { return this[index]; }
            set { this[index] = (ExtensionConfigurationElement) value; }
        }
    }
}

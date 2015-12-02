using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DbKeeperNet.Engine.Configuration
{
    public class ExtensionCollection :
        IExtensionConfigurationElementCollection
    {
        private List<IExtensionConfigurationElement> _data = new List<IExtensionConfigurationElement>
        {
            new Extension {Assembly = typeof (IExtensionConfigurationElementCollection).GetTypeInfo().Assembly.FullName}
        };

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IExtensionConfigurationElement> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public IExtensionConfigurationElement this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public void Add(IExtensionConfigurationElement element)
        {
            _data.Add(element);
        }
    }
}
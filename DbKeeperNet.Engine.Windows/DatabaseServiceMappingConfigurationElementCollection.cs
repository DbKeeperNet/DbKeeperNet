using System.Collections.Generic;
using System.Configuration;

namespace DbKeeperNet.Engine.Windows
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public class DatabaseServiceMappingConfigurationElementCollection : ConfigurationElementCollection, IDatabaseServiceMappingConfigurationElementCollection
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

        IEnumerator<IDatabaseServiceMappingConfigurationElement> IEnumerable<IDatabaseServiceMappingConfigurationElement>.GetEnumerator()
        {
            foreach (IDatabaseServiceMappingConfigurationElement e in this)
            {
                yield return e;
            }
        }

        IDatabaseServiceMappingConfigurationElement IDatabaseServiceMappingConfigurationElementCollection.this[int index]
        {
            get { return this[index]; }
            set { this[index] = (DatabaseServiceMappingConfigurationElement) value; }
        }
    }
}

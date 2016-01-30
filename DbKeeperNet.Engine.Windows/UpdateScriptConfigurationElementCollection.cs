using System.Collections.Generic;
using System.Configuration;

namespace DbKeeperNet.Engine.Windows
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public class UpdateScriptConfigurationElementCollection : ConfigurationElementCollection, IUpdateScriptConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UpdateScriptConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UpdateScriptConfigurationElement)element).Location;
        }

        public UpdateScriptConfigurationElement this[int index]
        {
            get
            {
                return (UpdateScriptConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }

        }

        IEnumerator<IUpdateScriptConfigurationElement> IEnumerable<IUpdateScriptConfigurationElement>.GetEnumerator()
        {
            foreach (IUpdateScriptConfigurationElement e in this)
            {
                yield return e;
            }
        }

        IUpdateScriptConfigurationElement IUpdateScriptConfigurationElementCollection.this[int index]
        {
            get { return this[index]; }
            set { this[index] = (UpdateScriptConfigurationElement) value; }
        }
    }
}

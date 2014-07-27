using System.Configuration;

namespace DbKeeperNet.Engine
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
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
    
    }
}

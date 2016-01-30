using System.Collections;
using System.Collections.Generic;

namespace DbKeeperNet.Engine.Configuration
{
    public class DatabaseServiceMappingConfigurationElementCollection :
        IDatabaseServiceMappingConfigurationElementCollection
    {
        private readonly List<IDatabaseServiceMappingConfigurationElement> _data =
            new List<IDatabaseServiceMappingConfigurationElement>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IDatabaseServiceMappingConfigurationElement> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public IDatabaseServiceMappingConfigurationElement this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public void Add(IDatabaseServiceMappingConfigurationElement element)
        {
            _data.Add(element);
        }
    }
}
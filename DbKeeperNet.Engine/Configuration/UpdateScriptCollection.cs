using System.Collections;
using System.Collections.Generic;

namespace DbKeeperNet.Engine.Configuration
{
    public class UpdateScriptCollection :
        IUpdateScriptConfigurationElementCollection
    {
        private List<IUpdateScriptConfigurationElement> _data = new List<IUpdateScriptConfigurationElement>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IUpdateScriptConfigurationElement> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public IUpdateScriptConfigurationElement this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public void Add(IUpdateScriptConfigurationElement element)
        {
            _data.Add(element);
        }
    }
}
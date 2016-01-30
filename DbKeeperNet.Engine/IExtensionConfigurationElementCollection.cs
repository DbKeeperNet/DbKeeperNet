using System.Collections.Generic;

namespace DbKeeperNet.Engine
{
    public interface IExtensionConfigurationElementCollection : IEnumerable<IExtensionConfigurationElement>
    {
        IExtensionConfigurationElement this[int index] { get; set; }
    }
}
using System.Collections.Generic;

namespace DbKeeperNet.Engine
{
    public interface IDatabaseServiceMappingConfigurationElementCollection : IEnumerable<IDatabaseServiceMappingConfigurationElement>
    {
        IDatabaseServiceMappingConfigurationElement this[int index] { get; set; }
    }
}
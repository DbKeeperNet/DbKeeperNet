using System.Collections.Generic;

namespace DbKeeperNet.Engine
{
    public interface IUpdateScriptConfigurationElementCollection : IEnumerable<IUpdateScriptConfigurationElement>
    {
        IUpdateScriptConfigurationElement this[int index] { get; set; }
    }
}
using System.IO;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Contract defining service for script
    /// execution.
    /// </summary>
    public interface IScriptProviderService
    {
        /// <summary>
        /// Method should open script based
        /// on the given location
        /// </summary>
        Stream GetScriptStreamFromLocation();
    }
}
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
        /// Service registration name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Method should open script based
        /// on the given location
        /// </summary>
        /// <param name="location">Location of this script. Location format depends
        /// on the used provider</param>
        Stream GetScriptStreamFromLocation(string location);
    }
}

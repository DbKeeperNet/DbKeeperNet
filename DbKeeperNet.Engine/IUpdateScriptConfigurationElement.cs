namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Describes automatically executed upgrade script
    /// </summary>
    public interface IUpdateScriptConfigurationElement
    {
        /// <summary>
        /// Script location
        /// </summary>
        string Location { get; }

        /// <summary>
        /// Script provider to be used for <see cref="Location"/>
        /// resolution
        /// </summary>
        string Provider { get; }
    }
}
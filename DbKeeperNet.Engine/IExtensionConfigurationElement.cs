namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Automatically loaded extension configuration element
    /// </summary>
    public interface IExtensionConfigurationElement
    {
        /// <summary>
        /// Assembly to be loaded
        /// </summary>
        string Assembly { get; }
    }
}
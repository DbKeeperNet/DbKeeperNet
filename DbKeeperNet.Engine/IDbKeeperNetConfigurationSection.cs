namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Top level configuration
    /// </summary>
    public interface IDbKeeperNetConfigurationSection
    {
        /// <summary>
        /// Database connection string to database services mappings
        /// </summary>
        /// <see cref="IDatabaseService"/>
        /// <see cref="IDatabaseServiceMappingConfigurationElement"/>
        IDatabaseServiceMappingConfigurationElementCollection DatabaseServiceMappings { get; }

        /// <summary>
        /// Logging service to be used
        /// </summary>
        string LoggingService { get; }

        /// <summary>
        /// Extension assemblies to be loaded
        /// </summary>
        /// <see cref="IExtensionConfigurationElement"/>
        IExtensionConfigurationElementCollection Extensions { get; }

        /// <summary>
        /// Update scripts to be executed
        /// </summary>
        /// <see cref="IUpdateScriptConfigurationElement"/>
        IUpdateScriptConfigurationElementCollection UpdateScripts { get; }
    }
}
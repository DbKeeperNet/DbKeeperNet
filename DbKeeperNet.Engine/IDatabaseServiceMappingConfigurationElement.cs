namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Database connection string to database service
    /// mapping element
    /// </summary>
    public interface IDatabaseServiceMappingConfigurationElement
    {
        /// <summary>
        /// Connection string identifier
        /// </summary>
        string ConnectString { get; }

        /// <summary>
        /// Database service registration string
        /// </summary>
        /// <see cref="IDatabaseService.Name"/>
        string DatabaseService { get; }
    }
}
namespace DbKeeperNet.Engine.Configuration
{
    public class DatabaseServiceMappingConfigurationElement : IDatabaseServiceMappingConfigurationElement
    {
        public string ConnectString { get; set; }
        public string DatabaseService { get; set; }
    }
}
namespace DbKeeperNet.Engine.Configuration
{
    public class DbKeeperNetConfigurationX : IDbKeeperNetConfigurationSection
    {
        private readonly DatabaseServiceMappingConfigurationElementCollection
            _databaseServiceMappingConfigurationElementCollection =
                new DatabaseServiceMappingConfigurationElementCollection();

        private readonly ExtensionCollection _extensions = new ExtensionCollection();
        private readonly UpdateScriptCollection _updateScripts = new UpdateScriptCollection();


        IDatabaseServiceMappingConfigurationElementCollection IDbKeeperNetConfigurationSection.DatabaseServiceMappings
        {
            get { return _databaseServiceMappingConfigurationElementCollection; }
        }

        public string LoggingService { get; set; }
        
        IExtensionConfigurationElementCollection IDbKeeperNetConfigurationSection.Extensions
        {
            get { return _extensions; }
        }

        IUpdateScriptConfigurationElementCollection IDbKeeperNetConfigurationSection.UpdateScripts
        {
            get { return _updateScripts; }
        }

        public DatabaseServiceMappingConfigurationElementCollection DatabaseServiceMappings
        {
            get { return _databaseServiceMappingConfigurationElementCollection; }
        }

        public ExtensionCollection Extensions
        {
            get { return _extensions; }
        }

        public UpdateScriptCollection UpdateScripts
        {
            get { return _updateScripts; }
        }
    }
}
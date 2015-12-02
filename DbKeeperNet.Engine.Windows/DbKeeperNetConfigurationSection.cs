using System.Configuration;

namespace DbKeeperNet.Engine.Windows
{
    /// <summary>Configuration section for .NET App.Config and Web.Config files</summary>
    /// <remarks>
    /// <code>
    /// <![CDATA[
    /// <?xml version="1.0" encoding="utf-8" ?>
    /// <configuration>
    ///   <configSections>
    ///     <section name="dbkeeper.net" type="DbKeeperNet.Engine.Windows.DbKeeperNetConfigurationSection,DbKeeperNet.Engine.Windows"/>
    ///   </configSections>
    ///   <dbkeeper.net loggingService="fx">
    ///       <databaseServiceMappings>
    ///         <add connectString="mssql" databaseService="MsSql" />
    ///       </databaseServiceMappings>
    ///       <extensions>
    ///         <add assembly="DbKeeperNet.dll" />
    ///       </extensions>
    ///       <updateScripts>
    ///         <add provider="asm" location="manifestResource="MyAssembly.Updates.xml,MyAssembly.dll" />
    ///         <add provider="disk" location="update.xml" />
    ///       </updateScripts>
    ///   </dbkeeper.net>
    /// </configuration>
    /// ]]>
    /// </code>
    /// </remarks>
    public sealed class DbKeeperNetConfigurationSection: ConfigurationSection, IDbKeeperNetConfigurationSection
    {
        [ConfigurationProperty("databaseServiceMappings")]
        public DatabaseServiceMappingConfigurationElementCollection DatabaseServiceMappings
        {
            get
            {
                return (DatabaseServiceMappingConfigurationElementCollection)this["databaseServiceMappings"];
            }
        }

        [ConfigurationProperty("loggingService", DefaultValue = "dummy")]
        public string LoggingService
        {
            get { return (string)this["loggingService"]; }
        }

        [ConfigurationProperty("extensions")]
        public ExtensionConfigurationElementCollection Extensions
        {
            get { return (ExtensionConfigurationElementCollection)this["extensions"]; }
        }
        [ConfigurationProperty("updateScripts")]
        public UpdateScriptConfigurationElementCollection UpdateScripts
        {
            get { return (UpdateScriptConfigurationElementCollection)this["updateScripts"]; }
        }
        public static DbKeeperNetConfigurationSection Current
        {
            get
            {
                return (DbKeeperNetConfigurationSection)ConfigurationManager.GetSection("dbkeeper.net");
            }
        }

        IDatabaseServiceMappingConfigurationElementCollection IDbKeeperNetConfigurationSection.DatabaseServiceMappings
        {
            get { return DatabaseServiceMappings; }
        }

        IExtensionConfigurationElementCollection IDbKeeperNetConfigurationSection.Extensions
        {
            get { return Extensions; }
        }

        IUpdateScriptConfigurationElementCollection IDbKeeperNetConfigurationSection.UpdateScripts
        {
            get { return UpdateScripts; }
        }
    }
}

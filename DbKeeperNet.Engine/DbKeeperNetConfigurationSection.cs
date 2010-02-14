using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// <code>
    /// <![CDATA[
    /// <?xml version="1.0" encoding="utf-8" ?>
    /// <configuration>
    ///   <configSections>
    ///     <section name="dbkeeper.net" type="DbKeeperNet.Engine.DbKeeperNetConfigurationSection,DbKeeperNet.Engine"/>
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
    /// </summary>
    public sealed class DbKeeperNetConfigurationSection: ConfigurationSection
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
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// <![CDATA[
    /// <?xml version="1.0" encoding="utf-8" ?>
    /// <configuration>
    ///   <configSections>
    ///       <section name="dbkeeper.net" type="DbUpdater.Engine.AppUpdaterConfiguration,DbUpdater.Engine"/>
    ///   </configSections>
    ///   <dbkeeper.net loggingService="fx">
    ///       <databaseServiceMappings>
    ///         <add connectString="mock" type="MockDriver" />
    ///       </databaseServiceMappings>
    ///   </dbkeeper.net>
    ///   </configuration>
    /// ]]>
    /// </summary>
    public sealed class DbKeeperNetConfigurationSection: ConfigurationSection
    {
        [ConfigurationProperty("databaseServiceMappings", IsKey = true)]
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
        [ConfigurationProperty("assemblyUpdates")]
        public AssemblyUpdateConfigurationElementCollection AssemblyUpdates
        {
            get { return (AssemblyUpdateConfigurationElementCollection)this["assemblyUpdates"]; }
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

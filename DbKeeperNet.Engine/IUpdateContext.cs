using System;
using System.IO;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Interface declaring current update execution
    /// context.
    /// </summary>
    public interface IUpdateContext : IDisposable
    {
        /// <summary>
        /// Currently running update version (taken from Update XML element).
        /// </summary>
        string CurrentVersion { get; set; }
        
        /// <summary>
        /// Currently running update version step (taken from Update/UpdateStep[@Id] attribute).
        /// </summary>
        int CurrentStep { get; set; }
        
        /// <summary>
        /// Application unique update group identifier (can be namespace, GUID etc.)
        /// </summary>
        string CurrentAssemblyName { get; set; }
        
        /// <summary>
        /// Database services attached to this execution context. Database connection
        /// should be closed during disposing of this context instance.
        /// </summary>
        IDatabaseService DatabaseService { get; }
        
        /// <summary>
        /// Service which allows message logging
        /// </summary>
        ILoggingService Logger { get; }
        
        /// <summary>
        /// Verify precondition identified by <paramref name="name"/>
        /// with optional parameter <paramref name="parameters"/>.
        /// </summary>
        /// <param name="name">Precondition identifier.</param>
        /// <param name="parameters">Optional parameter</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>true - condition was met, step can be executed.</item>
        /// <item>false - prevent step from execution.</item>
        /// </list>
        /// </returns>
        bool CheckPrecondition(string name, PreconditionParamType[] parameters);
        
        /// <summary>
        /// Method called by extension to register a precondition
        /// plugin.
        /// </summary>
        /// <param name="precondition">Instance of precondition</param>
        void RegisterPrecondition(IPrecondition precondition);
        
        /// <summary>
        /// Method called by extension to register a database service
        /// plugin.
        /// </summary>
        /// <param name="service">Instance of database service</param>
        void RegisterDatabaseService(IDatabaseService service);
        
        /// <summary>
        /// Method called by extension to register a logging service
        /// plugin.
        /// </summary>
        /// <param name="service">Instance of the logging service</param>
        void RegisterLoggingService(ILoggingService service);
        
        /// <summary>
        /// Method called by extension to register a script providing service
        /// plugin.
        /// </summary>
        /// <param name="provider">Instance of the script execution provider</param>
        void RegisterScriptProviderService(IScriptProviderService provider);
        
        /// <summary>
        /// Initialize context database service based on given
        /// connection string name from App.Config.
        /// 
        /// Given connection string name must mu correctly mapped in App.Config section:
        /// <code>
        /// <![CDATA[
        /// <dbkeeper.net loggingService="fx">
        ///   <databaseServiceMappings>
        ///     <add connectString="mock" databaseService="MsSql" />
        /// ]]>
        /// </code>
        /// </summary>
        /// <param name="connectionString">Connection string name within App.Config</param>
        void InitializeDatabaseService(string connectionString);

        /// <summary>
        /// Initialize database service by passing an instance of <see cref="IDatabaseService"/>.
        /// </summary>
        /// <remarks>Database service is not considered as owned </remarks>
        /// <param name="databaseService">Instance of database service.</param>
        /// <param name="disposeService"><c>true</c> if database service should be disposed when the context is disposed.</param>
        void InitializeDatabaseService(IDatabaseService databaseService, bool disposeService);

        /// <summary>
        /// Force manual logging service initialization based on its name.
        /// If this method is not called, logging service from App.Config
        /// should be automatically initialized at the moment it's first time
        /// used:
        /// <code><![CDATA[<dbkeeper.net loggingService="fx">]]></code>
        /// </summary>
        /// <param name="serviceName">Logging service registration name (such as fx, dummy etc.)</param>
        void InitializeLoggingService(string serviceName);
        
        /// <summary>
        /// Default preconditions applied to the update step
        /// in the case that no step specific preconditions
        /// are declared.
        /// </summary>
        PreconditionType[] DefaultPreconditions { get; set; }
        
        /// <summary>
        /// Load all necessary extensions and call appropriate
        /// initialization method to allow services registration
        /// for them.
        /// </summary>
        void LoadExtensions();
        
        /// <summary>
        /// Configuration section of the DbKeeperNet.
        /// By default should refer to the App.Config.
        /// </summary>
        DbKeeperNetConfigurationSection ConfigurationSection { get; }

        /// <summary>
        /// Gets all registered script execution services.
        /// </summary>
        /// <param name="provider">Name of the registered provider service</param>
        /// <param name="location">Location parameter for the service</param>
        Stream GetScriptFromStreamLocation(string provider, string location);
    }
}

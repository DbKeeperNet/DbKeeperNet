using System;
using System.Collections.Generic;
using DbKeeperNet.Engine.Resources;
using System.Globalization;
using System.IO;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Default IUpdateContext interface implementation.
    /// </summary>
    public class UpdateContext : DisposableObject, IUpdateContext
    {
        private IDatabaseService _databaseService;
        private ILoggingService _loggingService;
        private readonly DbKeeperNetConfigurationSection _configurationSection;

        private readonly Dictionary<string, ILoggingService> _loggingServices = new Dictionary<string, ILoggingService>(StringComparer.Ordinal);
        private readonly Dictionary<string, IPrecondition> _preconditions = new Dictionary<string, IPrecondition>(StringComparer.Ordinal);
        private readonly Dictionary<string, IDatabaseService> _databaseServices = new Dictionary<string, IDatabaseService>(StringComparer.Ordinal);
        private readonly Dictionary<string, IScriptProviderService> _scriptProviderServices = new Dictionary<string, IScriptProviderService>(StringComparer.Ordinal);
        private readonly IDictionary<Type, IUpdateStepHandlerService> _updateStepHandlerServices = new Dictionary<Type, IUpdateStepHandlerService>();

        private string _friendlyName;
        private string _currentAssemblyName;
        private string _currentVersion;
        private int _currentStep;

        private PreconditionType[] _defaultPreconditions = { };
        private bool _databaseServiceOwner;
        private readonly IDictionary<string, string> _schemas = new Dictionary<string, string>();

        public UpdateContext()
            : this(DbKeeperNetConfigurationSection.Current)
        {
        }

        public UpdateContext(DbKeeperNetConfigurationSection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

             _configurationSection = section;
        }

        #region IUpdateContext Members
        public ILoggingService Logger
        {
            get
            {
                if (_loggingService == null)
                    InitializeLoggingService(_configurationSection.LoggingService);

                if (_loggingService == null)
                    throw new InvalidOperationException(UpdateContextMessages.CanNotInitializeLoggingService);

                return _loggingService;
            }
        }

        public IDatabaseService DatabaseService
        {
            get
            {
                if (_databaseService == null)
                    throw new InvalidOperationException(UpdateContextMessages.DatabaseServiceNotInitialized);

                return _databaseService;
            }
        }

        public void RegisterPrecondition(IPrecondition precondition)
        {
            if (precondition == null)
                throw new ArgumentNullException(@"precondition");
            if (String.IsNullOrEmpty(precondition.Name))
                throw new InvalidOperationException(UpdateContextMessages.PreconditionNameNull);

            if (_preconditions.ContainsKey(precondition.Name))
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, UpdateContextMessages.PreconditionAlreadyRegistered, precondition.Name));

            _preconditions[precondition.Name] = precondition;
        }

        public bool CheckPrecondition(string name, PreconditionParamType[] parameters)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            IPrecondition precondition = _preconditions[name];

            return precondition.CheckPrecondition(this, parameters);
        }

        public void RegisterDatabaseService(IDatabaseService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            if (String.IsNullOrEmpty(service.Name))
                throw new InvalidOperationException(UpdateContextMessages.DatabaseServiceNameNull);

            if (_databaseServices.ContainsKey(service.Name))
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, UpdateContextMessages.DatabaseServiceAlreadyRegistered, service.Name));

            _databaseServices[service.Name] = service;
        }

        public void RegisterLoggingService(ILoggingService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            if (String.IsNullOrEmpty(service.Name))
                throw new InvalidOperationException(UpdateContextMessages.LoggingServiceNameNull);

            if (_loggingServices.ContainsKey(service.Name))
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, UpdateContextMessages.LoggingServiceAlreadyRegistered, service.Name));

            _loggingServices[service.Name] = service;
        }

        public void RegisterScriptProviderService(IScriptProviderService provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (String.IsNullOrEmpty(provider.Name))
                throw new InvalidCastException(UpdateContextMessages.ScriptExecutionServiceNameNull);

            if (_loggingServices.ContainsKey(provider.Name))
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, UpdateContextMessages.LoggingServiceAlreadyRegistered, provider.Name));

            _scriptProviderServices[provider.Name] = provider;
        }

        public void RegisterUpdateStepHandler(IUpdateStepHandlerService handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");

            _updateStepHandlerServices[handler.HandledType] = handler;
        }

        public void RegisterSchema(string schemaNamespace, string schema)
        {
            if (string.IsNullOrEmpty(schemaNamespace)) throw new ArgumentNullException("schemaNamespace");
            if (string.IsNullOrEmpty(schema)) throw new ArgumentNullException("schema");

            // do not support re-registration
            _schemas.Add(schemaNamespace, schema);
        }

        public void IterateAllSchemas(Action<string, string> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");

            foreach (var schemaInfo in _schemas) callback(schemaInfo.Key, schemaInfo.Value);
        }

        public IUpdateStepHandlerService GetUpdateStepHandlerFor(UpdateStepBaseType updateStep)
        {
            if (updateStep == null) throw new ArgumentNullException("updateStep");

            return _updateStepHandlerServices[updateStep.GetType()];
        }

        public string CurrentVersion
        {
            get
            {
                return _currentVersion;
            }
            set
            {
                _currentVersion = value;
            }
        }

        public int CurrentStep
        {
            get
            {
                return _currentStep;
            }
            set
            {
                _currentStep = value;
            }
        }

        public string CurrentAssemblyName
        {
            get
            {
                return _currentAssemblyName;
            }
            set
            {
                _currentAssemblyName = value;
            }
        }

        public string FriendlyName
        {
            get
            {
                return _friendlyName;
            }
            set
            {
                _friendlyName = value;
            }
        }
        public PreconditionType[] DefaultPreconditions
        {
            get { return _defaultPreconditions; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _defaultPreconditions = value;
            }
        }

        public void InitializeLoggingService(string serviceName)
        {
            if (String.IsNullOrEmpty(serviceName))
                throw new ArgumentNullException("serviceName");

            if (!_loggingServices.ContainsKey(serviceName))
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, UpdateContextMessages.LoggingServiceNotRegistered, serviceName));

            _loggingService = _loggingServices[serviceName];
        }

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
        public void InitializeDatabaseService(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            string databaseServiceName = null;

            Logger.TraceInformation(UpdateContextMessages.SearchingDatabaseService, connectionString);

            foreach (DatabaseServiceMappingConfigurationElement e in ConfigurationSection.DatabaseServiceMappings)
            {
				if (connectionString.Equals(e.ConnectString, StringComparison.Ordinal))
                {
                    databaseServiceName = e.DatabaseService;
                    break;
                }
            }

            if (databaseServiceName == null)
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, UpdateContextMessages.DatabaseServiceMappingNotFound, connectionString));

            if (!_databaseServices.ContainsKey(databaseServiceName))
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, UpdateContextMessages.DatabaseServiceNotRegistered, databaseServiceName));

            Logger.TraceInformation(String.Format(CultureInfo.CurrentCulture, UpdateContextMessages.DatabaseServiceMappingFound, connectionString, databaseServiceName));

            _databaseService = _databaseServices[databaseServiceName].CloneForConnectionString(connectionString);
            _databaseServiceOwner = true;

            Logger.TraceInformation(UpdateContextMessages.DatabaseServiceInitialized, connectionString);
        }

        /// <summary>
        /// Initialize database service by passing an instance of <see cref="IDatabaseService"/>.
        /// </summary>
        /// <remarks>Database service is not considered as owned </remarks>
        /// <param name="databaseService">Instance of database service.</param>
        /// <param name="disposeService"><c>true</c> if database service should be disposed when the context is disposed.</param>
        public void InitializeDatabaseService(IDatabaseService databaseService, bool disposeService)
        {
            if (databaseService == null)
                throw new ArgumentNullException(@"databaseService");

            _databaseService = databaseService;
            _databaseServiceOwner = disposeService;

            Logger.TraceInformation(String.Format(CultureInfo.CurrentCulture, UpdateContextMessages.InitializedUsingInstantiatedDatabaseService, databaseService));
        }

        public void LoadExtensions()
        {
            ExtensionLoader.LoadExtensions(this);
        }

        public DbKeeperNetConfigurationSection ConfigurationSection
        {
            get
            {
                return _configurationSection;
            }
        }

        public Stream GetScriptFromStreamLocation(string provider, string location)
        {
            if (String.IsNullOrEmpty(provider))
                throw new ArgumentNullException("provider");
            if (String.IsNullOrEmpty(location))
                throw new ArgumentNullException("location");

            IScriptProviderService service = _scriptProviderServices[provider];

            return service.GetScriptStreamFromLocation(location);
        }
        #endregion

        /// <summary>
        /// Invoked from <seealso cref="DisposableObject.Dispose()"/> method
        /// and finalizer.
        /// </summary>
        /// <param name="disposing">Indicates, whether this has been invoked from finalizer or <seealso cref="IDisposable.Dispose"/>.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (disposing)
                    {
                        if ((_databaseService != null) && (_databaseServiceOwner))
                            _databaseService.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(true);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Default IUpdateContext interface implementation.
    /// </summary>
    public class UpdateContext : IUpdateContext
    {
        IDatabaseService _databaseService;
        ILoggingService _loggingService;

        Dictionary<string, ILoggingService> _loggingServices = new Dictionary<string, ILoggingService>();
        Dictionary<string, IPrecondition> _preconditions = new Dictionary<string, IPrecondition>();
        Dictionary<string, IDatabaseService> _databaseServices = new Dictionary<string, IDatabaseService>();

        string _friendlyName;
        string _currentAssemblyName;
        string _currentVersion;
        int _currentStep;
        PreconditionType[] _defaultPreconditions = { };

        public UpdateContext()
        {
        }

        #region IUpdateContext Members
        public ILoggingService Logger
        {
            get
            {
                if (_loggingService == null)
                    InitializeLoggingService(DbKeeperNetConfigurationSection.Current.LoggingService);

                if (_loggingService == null)
                    throw new InvalidOperationException("Logging service can't be initialized");

                return _loggingService;
            }
        }
        public IDatabaseService DatabaseService
        {
            get
            {
                if (_databaseService == null)
                    throw new InvalidOperationException("Database driver is not initialized");

                return _databaseService;
            }
        }

        public void RegisterPrecondition(IPrecondition precondition)
        {
            if (precondition == null)
                throw new ArgumentNullException(@"precondition");
            if (String.IsNullOrEmpty(precondition.Name))
                throw new ArgumentNullException(@"precondition.Name");

            if (_preconditions.ContainsKey(precondition.Name))
                throw new InvalidOperationException(String.Format("Precondition '{0}' already registered", precondition.Name));

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
                throw new ArgumentNullException("service.Name");

            if (_databaseServices.ContainsKey(service.Name))
                throw new InvalidOperationException(String.Format("Database service '{0}' already registered", service.Name));

            _databaseServices[service.Name] = service;
        }

        public void RegisterLoggingService(ILoggingService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            if (String.IsNullOrEmpty(service.Name))
                throw new ArgumentNullException("service.Name");

            if (_loggingServices.ContainsKey(service.Name))
                throw new InvalidOperationException(String.Format("Logging service '{0}' already registered", service.Name));

            _loggingServices[service.Name] = service;
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
                throw new InvalidOperationException(String.Format("Logging service '{0}' is not registered", serviceName));

            _loggingService = _loggingServices[serviceName];
        }

        public void InitializeDatabaseService(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            string databaseServiceName = null;

            Logger.TraceInformation("Searching database service for connection string '{0}'", connectionString);

            foreach (DatabaseServiceMappingConfigurationElement e in DbKeeperNetConfigurationSection.Current.DatabaseServiceMappings)
            {
                if (e.ConnectString == connectionString)
                {
                    databaseServiceName = e.DatabaseService;
                    break;
                }
            }

            if (databaseServiceName == null)
                throw new InvalidOperationException(String.Format("Database service mapping for connection string '{0}' not found", connectionString));

            if (!_databaseServices.ContainsKey(databaseServiceName))
                throw new InvalidOperationException(String.Format("Database service '{0}' not registered", databaseServiceName));

            Logger.TraceInformation(String.Format("For connection string '{0}' found database service '{1}'", connectionString, databaseServiceName));

            _databaseService = _databaseServices[databaseServiceName].CloneForConnectionString(connectionString);

            Logger.TraceInformation("Initialized database service for connection string '{0}'", connectionString);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_databaseService != null)
                {
                    _databaseService.Dispose();
                    _databaseService = null;
                }
            }
        }
        #endregion
    }
}

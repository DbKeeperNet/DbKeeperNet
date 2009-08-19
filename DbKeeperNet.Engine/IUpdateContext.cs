using System;
using System.Collections.Generic;
using System.Text;

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
        /// Database services attached to this execution context.
        /// </summary>
        IDatabaseService DatabaseService { get; }
        /// <summary>
        /// Service which allows message logging
        /// </summary>
        ILoggingService Logger { get; }
        /// <summary>
        /// Verify precondition identified by <paramref name="name"/>
        /// with optional parameter <paramref name="param"/>.
        /// </summary>
        /// <param name="name">Precondition identifier.</param>
        /// <param name="parameters">Optional parameter</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>true - condition was met, step can be executed.</item>
        /// <item>false - prevent step from execution.</item>
        /// </list>
        /// </returns>
        bool CheckPrecondition(string name, string[] parameters);
        /// <summary>
        /// Method called by extension to register a precondition
        /// plugin.
        /// </summary>
        /// <param name="precondition">Instance of precondition</param>
        void RegisterPrecondition(IPrecondition precondition);
        void RegisterDatabaseService(IDatabaseService service);
        void RegisterLoggingService(ILoggingService service);

        void InitializeDatabaseService(string connectionString);
        void InitializeLoggingService(string serviceName);
        /// <summary>
        /// Default preconditions applied to the update step
        /// in the case that no step specific preconditions
        /// are declared.
        /// </summary>
        PreconditionType[] DefaultPreconditions { get; set; }
    }
}

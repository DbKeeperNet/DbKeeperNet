using System;
using System.Data.Common;
using System.IO;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Database service common interface. When registering database service
    /// an instance without database connection. When the connection has to
    /// be initialized, clone for connection string is called. At this
    /// moment should be new database connection opened.
    /// </summary>
    public interface IDatabaseService : IDisposable
    {
        /// <summary>
        /// Direct access to established database connection.
        /// Connection should be closed when this service
        /// instance is disposed.
        /// </summary>
        DbConnection Connection { get; }
        /// <summary>
        /// Verify whether given table name already exists
        /// in database
        /// </summary>
        /// <param name="tableName">Database table name</param>
        /// <returns>true when exists, false otherwise</returns>
        bool TableExists(string tableName);
        /// <summary>
        /// Verify whether given stored procedure already exists
        /// in database
        /// </summary>
        /// <param name="procedureName">Stored procedure name</param>
        /// <returns>true when exists, false otherwise</returns>
        bool StoredProcedureExists(string procedureName);
        /// <summary>
        /// Verify whether given view already exists
        /// in database
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <returns>true when exists, false otherwise</returns>
        bool ViewExists(string viewName);
        /// <summary>
        /// Verify whether given primary key or index already exists
        /// in database
        /// </summary>
        /// <param name="indexName">Primary key or index name</param>
        /// <param name="table">Table on which we are checking index</param>
        /// <returns>true when exists, false otherwise</returns>
        bool IndexExists(string indexName, string table);
        /// <summary>
        /// Verify whether given foreign key already exists
        /// in database
        /// </summary>
        /// <param name="foreignKeyName">Foreign key name</param>
        /// <param name="table">Table on which we are checking foreign key</param>
        /// <returns>true when exists, false otherwise</returns>
        bool ForeignKeyExists(string foreignKeyName, string table);
        /// <summary>
        /// Verify whether given primary key on table already exists
        /// in database
        /// </summary>
        /// <param name="primaryKeyName">Primary key name</param>
        /// <param name="table">Table on which we are checking primary key</param>
        /// <returns>true when exists, false otherwise</returns>
        bool PrimaryKeyExists(string primaryKeyName, string table);
        /// <summary>
        /// Verify whether given trigger already exists
        /// in database
        /// </summary>
        /// <param name="triggerName">Trigger name</param>
        /// <returns>true when exists, false otherwise</returns>
        bool TriggerExists(string triggerName);
        /// <summary>
        /// Clone this object and establish a database connection
        /// with specified connection string.
        /// </summary>
        /// <param name="connectionString">Connection string from App.Config file</param>
        /// <returns>Cloned database service with initialized database connection.</returns>
        IDatabaseService CloneForConnectionString(string connectionString);
        /// <summary>
        /// Database service name as it may be referenced in configuration
        /// file. This is not value for XML update definition (although in fact
        /// the value itself may be the same).
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Check whether update step identified by assembly, version
        /// and step was already executed or not. This function is indented
        /// to operate on a database table.
        /// </summary>
        /// <param name="assemblyName">Assembly name as it's defined in update XML file.</param>
        /// <param name="version">Current update version.</param>
        /// <param name="stepNumber">Update step within the version.</param>
        /// <returns>true in the case that step was already executed, false otherwise.</returns>
        /// <see cref="SetUpdateStepExecuted"/>
        bool IsUpdateStepExecuted(string assemblyName, string version, int stepNumber);
        /// <summary>
        /// Request marking update step identified by assembly, version
        /// and step as already executed. This function is indented
        /// to operate on a persistent storage such as a table(s) 
        /// containing information from parameters.
        /// </summary>
        /// <param name="assemblyName">Assembly name as it's defined in update XML file.</param>
        /// <param name="version">Current update version.</param>
        /// <param name="stepNumber">Update step within the version.</param>
        /// <see cref="IsUpdateStepExecuted"/>
        void SetUpdateStepExecuted(string assemblyName, string version, int stepNumber);
        /// <summary>
        /// Should execute direct SQL command in parametr
        /// </summary>
        /// <param name="sql">Valid SQL statement</param>
        void ExecuteSql(string sql);
        /// <summary>
        /// Returns database setup XML script, if applicable,
        /// which will be run as prerequisity before all
        /// updates.
        /// </summary>
        /// <returns>null if not applicable, opened stream otherwise.</returns>
        Stream DatabaseSetupXml { get; }
        /// <summary>
        /// Begin a new database transaction related to this
        /// database connection.
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// Commit an already running transaction.
        /// </summary>
        void CommitTransaction();
        /// <summary>
        /// Perform rollback on an already running transaction.
        /// </summary>
        void RollbackTransaction();
        /// <summary>
        /// Returns true in the case that there is an already
        /// active database transaction for this connection.
        /// </summary>
        bool HasActiveTransaction { get; }
        /// <summary>
        /// Checks whether the given <paramref name="dbTypeName"/>
        /// is supported by this database service.
        /// </summary>
        /// <param name="dbTypeName">Database type as it is used in XML update definition.</param>
        /// <returns>true - this database service supports the given database type, false - doesn't support.</returns>
        bool IsDbType(string dbTypeName);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.IO;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Database service common interface.
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
        /// <param name="tableName">Stored procedure name</param>
        /// <returns>true when exists, false otherwise</returns>
        bool StoredProcedureExists(string procedureName);
        bool ViewExists(string viewName);
        bool IndexExists(string indexName);
        bool ForeignKeyExists(string foreignKeyName);

        IDatabaseService CloneForConnectionString(string connectionString);

        /// <summary>
        /// 
        /// </summary>
        string Name { get; }
        bool IsUpdateStepExecuted(string assemblyName, string version, int step);
        void SetUpdateStepExecuted(string assemblyName, string version, int step);
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
        Stream GetDatabaseSetupXml();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace DbKeeperNet.Engine
{
    public interface IDatabaseService : IDisposable
    {
        /// <summary>
        /// Direct access to established database connection.
        /// </summary>
        DbConnection Connection { get; }

        bool TableExists(string tableName);
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
    }
}

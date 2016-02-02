using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

namespace DbKeeperNet.Engine.Core.Extensions.DatabaseServices
{
    public class MsSqlDatabaseService : DisposableObject, IDatabaseService
    {
        private SqlConnection _connection;

        private MsSqlDatabaseService(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public object Connection { get { return _connection; } }

        public bool TableExists(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public bool StoredProcedureExists(string procedureName)
        {
            throw new System.NotImplementedException();
        }

        public bool ViewExists(string viewName)
        {
            throw new System.NotImplementedException();
        }

        public bool IndexExists(string indexName, string table)
        {
            throw new System.NotImplementedException();
        }

        public bool ForeignKeyExists(string foreignKeyName, string table)
        {
            throw new System.NotImplementedException();
        }

        public bool PrimaryKeyExists(string primaryKeyName, string table)
        {
            throw new System.NotImplementedException();
        }

        public bool TriggerExists(string triggerName)
        {
            throw new System.NotImplementedException();
        }

        public IDatabaseService CloneForConnectionString(string connectionString)
        {
            return new MsSqlDatabaseService(connectionString);
        }

        public string Name { get; }
        public bool IsUpdateStepExecuted(string assemblyName, string version, int stepNumber)
        {
            throw new System.NotImplementedException();
        }

        public void SetUpdateStepExecuted(string assemblyName, string version, int stepNumber)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteSql(string sql)
        {
            throw new System.NotImplementedException();
        }

        public Stream DatabaseSetupXml { get; }
        public void BeginTransaction()
        {
            throw new System.NotImplementedException();
        }

        public void CommitTransaction()
        {
            throw new System.NotImplementedException();
        }

        public void RollbackTransaction()
        {
            throw new System.NotImplementedException();
        }

        public bool HasActiveTransaction { get; }
        public bool IsDbType(string dbTypeName)
        {
            throw new System.NotImplementedException();
        }
    }
}
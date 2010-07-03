using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.DatabaseServices
{
    /// <summary>
    /// Database services for MsSQL server 2000 or higher.
    /// Service name for configuration file: MsSql
    /// </summary>
    public sealed class MsSqlDatabaseService : IDatabaseService
    {
        DbConnection _connection;
        DbTransaction _transaction;

        public MsSqlDatabaseService()
        {
        }

        private MsSqlDatabaseService(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            ConnectionStringSettings connectString = ConfigurationManager.ConnectionStrings[connectionString];

            if (connectionString == null)
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, DatabaseServiceMessages.ConnectionStringNotFound, connectionString));

            _connection = DbProviderFactories.GetFactory(connectString.ProviderName).CreateConnection();
            _connection.ConnectionString = connectString.ConnectionString;
            _connection.Open();
        }

        #region IDatabaseService Members

        public DbConnection Connection
        {
            get
            {
                if (_connection == null)
                    throw new InvalidOperationException(DatabaseServiceMessages.NotConnected);

                return _connection;
            }
        }

        public bool TableExists(string tableName)
        {
            if (String.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName");

            string[] restrictions = new string[4];

            restrictions[2] = tableName;
            restrictions[3] = "BASE TABLE";

            DataTable schema = Connection.GetSchema("Tables", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }

        public bool ViewExists(string viewName)
        {
            if (String.IsNullOrEmpty(viewName))
                throw new ArgumentNullException("viewName");

            string[] restrictions = new string[3];

            restrictions[2] = viewName;

            DataTable schema = Connection.GetSchema("Views", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }
        public bool IndexExists(string indexName, string table)
        {
            if (String.IsNullOrEmpty(indexName))
                throw new ArgumentNullException("indexName");

            string[] restrictions = new string[4];

            restrictions[3] = indexName;

            DataTable schema = Connection.GetSchema("Indexes", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }
        public bool ForeignKeyExists(string foreignKeyName, string table)
        {
            if (String.IsNullOrEmpty(foreignKeyName))
                throw new ArgumentNullException("foreignKeyName");

            string[] restrictions = new string[4];

            restrictions[3] = foreignKeyName;

            DataTable schema = Connection.GetSchema("ForeignKeys", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }
        public bool PrimaryKeyExists(string primaryKeyName, string table)
        {
            return IndexExists(primaryKeyName, table);
        }
        public bool TriggerExists(string triggerName)
        {
            if (String.IsNullOrEmpty(triggerName))
                throw new ArgumentNullException(@"triggerName");

            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture, @"select count(*) from sysobjects A where A.xtype='TR' and A.name = '{0}'", triggerName);

            bool exists = (Convert.ToInt64(cmd.ExecuteScalar(), CultureInfo.InvariantCulture) != 0);

            return exists;
        }
        public string Name
        {
            get { return @"MsSql"; }
        }

        public bool IsUpdateStepExecuted(string assemblyName, string version, int stepNumber)
        {
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "DbKeeperNetIsStepExecuted";
            cmd.CommandType = CommandType.StoredProcedure;

            DbParameter param = cmd.CreateParameter();
            param.ParameterName = "@assembly";
            param.Value = assemblyName;
            cmd.Parameters.Add(param);

            param = cmd.CreateParameter();
            param.ParameterName = "@version";
            param.Value = version;
            cmd.Parameters.Add(param);

            param = cmd.CreateParameter();
            param.ParameterName = "@step";
            param.Value = stepNumber;
            cmd.Parameters.Add(param);

            bool result = (bool)cmd.ExecuteScalar();

            return result;
        }

        public void SetUpdateStepExecuted(string assemblyName, string version, int stepNumber)
        {
            DbCommand cmd = Connection.CreateCommand();

            if (HasActiveTransaction)
                cmd.Transaction = _transaction;

            cmd.CommandText = "DbKeeperNetSetStepExecuted";
            cmd.CommandType = CommandType.StoredProcedure;

            DbParameter param = cmd.CreateParameter();
            param.ParameterName = "@assembly";
            param.Value = assemblyName;
            cmd.Parameters.Add(param);

            param = cmd.CreateParameter();
            param.ParameterName = "@version";
            param.Value = version;
            cmd.Parameters.Add(param);

            param = cmd.CreateParameter();
            param.ParameterName = "@step";
            param.Value = stepNumber;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();

        }

        public void ExecuteSql(string sql)
        {
            DbCommand cmd = Connection.CreateCommand();

            if (HasActiveTransaction)
                cmd.Transaction = _transaction;

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public bool StoredProcedureExists(string procedureName)
        {
            if (String.IsNullOrEmpty(procedureName))
                throw new ArgumentNullException("procedureName");

            string[] restrictions = new string[4];

            restrictions[2] = procedureName;
            restrictions[3] = "PROCEDURE";

            DataTable schema = Connection.GetSchema("Procedures", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }

        public IDatabaseService CloneForConnectionString(string connectionString)
        {
            return new MsSqlDatabaseService(connectionString);
        }

        public Stream DatabaseSetupXml
        {
            get { return Assembly.GetExecutingAssembly().GetManifestResourceStream(@"DbKeeperNet.Engine.Extensions.DatabaseServices.MsSqlDatabaseServiceInstall.xml"); }
        }
        public void BeginTransaction()
        {
            if (HasActiveTransaction)
                throw new InvalidOperationException(DatabaseServiceMessages.TransactionAlreadyInProgress);

            _transaction = _connection.BeginTransaction();
        }
        public void CommitTransaction()
        {
            if (!HasActiveTransaction)
                throw new InvalidOperationException(DatabaseServiceMessages.NoTransactionInProgressCanNotCommit);

            _transaction.Commit();
            _transaction = null;
        }
        public void RollbackTransaction()
        {
            if (!HasActiveTransaction)
                throw new InvalidOperationException(DatabaseServiceMessages.NoTransactionInProgressCanNotRollback);

            _transaction.Rollback();
            _transaction = null;
        }
        public bool HasActiveTransaction
        {
            get { return (_transaction != null); }
        }

        public bool IsDbType(string dbTypeName)
        {
            bool status = false;

            switch (dbTypeName.ToUpperInvariant())
            {
                case "MSSQL":
                    status = true;
                    break;
            }

            return status;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // we must release database connection
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }
        #endregion
    }
}

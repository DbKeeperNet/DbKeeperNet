using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.Common;
using System.Globalization;
using DbKeeperNet.Engine.Resources;
using System.Reflection;
using System.Data;
using System.Diagnostics;

namespace DbKeeperNet.Engine.Extensions.DatabaseServices
{
    /// <summary>
    /// Database services for SQLite 3 with ADO.NET provider.
    /// Service name for configuration file: SQLite
    /// </summary>
    /// <remarks>Available from SourceForge: http://sourceforge.net/projects/sqlite-dotnet2/</remarks>
    public sealed class SQLiteDatabaseService: IDatabaseService
    {
        public SQLiteDatabaseService()
        {
        }

        private SQLiteDatabaseService(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            ConnectionStringSettings connectString = ConfigurationManager.ConnectionStrings[connectionString];

            if (connectionString == null)
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, DatabaseServiceMessages.ConnectionStringNotFound, connectionString));

            _connection = DbProviderFactories.GetFactory(connectString.ProviderName).CreateConnection();
            _connection.ConnectionString = connectString.ConnectionString;
            _connection.Open();

            SetupDbCommands();
        }


        #region IDatabaseService Members

        public System.Data.Common.DbConnection Connection
        {
            get { return _connection; }
        }

        public bool TableExists(string tableName)
        {
            if (String.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName");

            string[] restrictions = new string[3];

            restrictions[2] = tableName;

            DataTable schema = Connection.GetSchema("Tables", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }

        public bool StoredProcedureExists(string procedureName)
        {
            if (String.IsNullOrEmpty(procedureName))
                throw new ArgumentNullException("procedureName");

            throw new NotSupportedException();
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

            restrictions[2] = table;
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

            restrictions[2] = table;

            DataTable schema = Connection.GetSchema("ForeignKeys", restrictions);

            bool result = false;

            foreach (DataRow row in schema.Rows)
            {
                if (row["CONSTRAINT_NAME"].ToString().StartsWith(foreignKeyName, StringComparison.OrdinalIgnoreCase))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool PrimaryKeyExists(string primaryKeyName, string table)
        {
            if (String.IsNullOrEmpty(primaryKeyName))
                throw new ArgumentNullException("primaryKeyName");

            if (String.IsNullOrEmpty(primaryKeyName))
                throw new ArgumentNullException("table");

            string[] restrictions = new string[3];

            restrictions[2] = table;
            
            DataTable schema = Connection.GetSchema("Indexes", restrictions);
            
            bool result = false;

            foreach (DataRow row in schema.Rows)
            {
                if (row["PRIMARY_KEY"].Equals(true) && (row["INDEX_NAME"].ToString().EndsWith(primaryKeyName, StringComparison.OrdinalIgnoreCase)))
                {
                    result = true;
                    break;
                }
            }
            
            return result;
        }
        
        public bool TriggerExists(string triggerName)
        {
            throw new NotSupportedException();
        }

        public IDatabaseService CloneForConnectionString(string connectionString)
        {
            return new SQLiteDatabaseService(connectionString);
        }

        public string Name
        {
            get { return @"SQLite"; }
        }

        public bool IsUpdateStepExecuted(string assemblyName, string version, int stepNumber)
        {
            bool result = false;

            _stepExecutedQuery.Parameters[0].Value = assemblyName;
            _stepExecutedQuery.Parameters[1].Value = version;
            _stepExecutedQuery.Parameters[2].Value = stepNumber;

            long? count = (long?)_stepExecutedQuery.ExecuteScalar();

            if ((count.HasValue) && (count.Value > 0))
                result = true;

            return result;
        }

        public void SetUpdateStepExecuted(string assemblyName, string version, int stepNumber)
        {
            if (HasActiveTransaction)
            {
                _assemblyInsert.Transaction = _transaction;
                _assemblySelect.Transaction = _transaction;
                _versionInsert.Transaction = _transaction;
                _versionSelect.Transaction = _transaction;
                _stepInsert.Transaction = _transaction;
                _stepSelect.Transaction = _transaction;
            }

            _assemblySelect.Parameters[0].Value = assemblyName;
            long? assemblyId = (long?)_assemblySelect.ExecuteScalar();

            if (!assemblyId.HasValue)
            {
                _assemblyInsert.Parameters[0].Value = assemblyName;
                assemblyId = Convert.ToInt64(_assemblyInsert.ExecuteScalar(), CultureInfo.InvariantCulture);
            }

            _versionSelect.Parameters[0].Value = assemblyId.Value;
            _versionSelect.Parameters[1].Value = version;
            long? versionId = (long?)_versionSelect.ExecuteScalar();

            if (!versionId.HasValue)
            {
                _versionInsert.Parameters[0].Value = assemblyId.Value;
                _versionInsert.Parameters[1].Value = version;
                versionId = Convert.ToInt64(_versionInsert.ExecuteScalar(), CultureInfo.InvariantCulture);
            }

            _stepSelect.Parameters[0].Value = versionId.Value;
            _stepSelect.Parameters[1].Value = stepNumber;
            long? stepId = (long?)_stepSelect.ExecuteScalar();

            if (!stepId.HasValue)
            {
                _stepInsert.Parameters[0].Value = versionId.Value;
                _stepInsert.Parameters[1].Value = stepNumber;
                _stepInsert.ExecuteNonQuery();
            }


        }

        public void ExecuteSql(string sql)
        {
            DbCommand cmd = Connection.CreateCommand();

            if (HasActiveTransaction)
                cmd.Transaction = _transaction;

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public System.IO.Stream DatabaseSetupXml
        {
            get { return Assembly.GetExecutingAssembly().GetManifestResourceStream(@"DbKeeperNet.Engine.Extensions.DatabaseServices.SQLiteDatabaseServiceInstall.xml"); }
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
                case "SQLITE":
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

        #region Private methods
        private void SetupDbCommands()
        {
            SetupAssemblyDbCommands();
            SetupVersionDbCommands();
            SetupStepDbCommands();

            if (_stepExecutedQuery == null)
            {
                _stepExecutedQuery = Connection.CreateCommand();
                _stepExecutedQuery.CommandText = @"select count(version)  from dbkeepernet_assembly asm
                    join dbkeepernet_version ver on asm.id = ver.dbkeepernet_assembly_id
                    join dbkeepernet_step step on ver.id = step.dbkeepernet_version_id
		            where step.step = @step 
                    and ver.version = @version and 
                    asm.assembly = @assembly";

                DbParameter assembly = _stepExecutedQuery.CreateParameter();
                assembly.ParameterName = "@assembly";
                DbParameter version = _stepExecutedQuery.CreateParameter();
                version.ParameterName = "@version";
                DbParameter step = _stepExecutedQuery.CreateParameter();
                step.ParameterName = "@step";

                _stepExecutedQuery.Parameters.Add(assembly);
                _stepExecutedQuery.Parameters.Add(version);
                _stepExecutedQuery.Parameters.Add(step);

            }
        }

        private void SetupAssemblyDbCommands()
        {
            if (_assemblySelect == null)
            {
                _assemblySelect = Connection.CreateCommand();
                _assemblySelect.CommandText = "select id from dbkeepernet_assembly where assembly = :assembly";

                DbParameter assembly = _assemblySelect.CreateParameter();
                assembly.ParameterName = ":assembly";

                _assemblySelect.Parameters.Add(assembly);
            }
            if (_assemblyInsert == null)
            {
                _assemblyInsert = Connection.CreateCommand();
                _assemblyInsert.CommandText = "insert into dbkeepernet_assembly(assembly, created) values(:assembly, CURRENT_TIMESTAMP); select last_insert_rowid()";

                DbParameter assembly = _assemblySelect.CreateParameter();
                assembly.ParameterName = ":assembly";

                _assemblyInsert.Parameters.Add(assembly);
            }
        }
        private void SetupVersionDbCommands()
        {
            if (_versionSelect == null)
            {
                _versionSelect = Connection.CreateCommand();
                _versionSelect.CommandText = "select id from dbkeepernet_version where dbkeepernet_assembly_id = :assemblyId and version = :version";

                DbParameter assemblyId = _assemblySelect.CreateParameter();
                assemblyId.ParameterName = ":assemblyId";

                DbParameter version = _assemblySelect.CreateParameter();
                version.ParameterName = ":version";

                _versionSelect.Parameters.Add(assemblyId);
                _versionSelect.Parameters.Add(version);
            }
            if (_versionInsert == null)
            {
                _versionInsert = Connection.CreateCommand();
                _versionInsert.CommandText = "insert into dbkeepernet_version(dbkeepernet_assembly_id, version, created) values(:assemblyId, :version, CURRENT_TIMESTAMP); select last_insert_rowid()";

                DbParameter assemblyId = _assemblySelect.CreateParameter();
                assemblyId.ParameterName = ":assemblyId";

                DbParameter version = _assemblySelect.CreateParameter();
                version.ParameterName = ":version";

                _versionInsert.Parameters.Add(assemblyId);
                _versionInsert.Parameters.Add(version);
            }
        }
        private void SetupStepDbCommands()
        {
            if (_stepSelect == null)
            {
                _stepSelect = Connection.CreateCommand();
                _stepSelect.CommandText = "select id from dbkeepernet_step where dbkeepernet_version_id = :versionId and step = :step";

                DbParameter versionId = _assemblySelect.CreateParameter();
                versionId.ParameterName = ":versionId";

                DbParameter step = _assemblySelect.CreateParameter();
                step.ParameterName = ":step";

                _stepSelect.Parameters.Add(versionId);
                _stepSelect.Parameters.Add(step);
            }
            if (_stepInsert == null)
            {
                _stepInsert = Connection.CreateCommand();
                _stepInsert.CommandText = "insert into dbkeepernet_step(dbkeepernet_version_id, step, created) values(:versionId, :step, CURRENT_TIMESTAMP); select last_insert_rowid()";

                DbParameter versionId = _assemblySelect.CreateParameter();
                versionId.ParameterName = ":versionId";

                DbParameter step = _assemblySelect.CreateParameter();
                step.ParameterName = ":step";

                _stepInsert.Parameters.Add(versionId);
                _stepInsert.Parameters.Add(step);
            }
        }
        #endregion

        #region Private members
        private DbConnection _connection;
        private DbTransaction _transaction;
        private DbCommand _assemblySelect;
        private DbCommand _assemblyInsert;
        private DbCommand _versionSelect;
        private DbCommand _versionInsert;
        private DbCommand _stepSelect;
        private DbCommand _stepInsert;
        private DbCommand _stepExecutedQuery;
        #endregion
    }
}

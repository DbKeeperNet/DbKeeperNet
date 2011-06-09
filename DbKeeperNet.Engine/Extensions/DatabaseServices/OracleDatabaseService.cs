using System;
using System.Data.Common;
using System.IO;
using System.Configuration;
using System.Globalization;
using DbKeeperNet.Engine.Resources;
using System.Data;
using System.Reflection;

namespace DbKeeperNet.Engine.Extensions.DatabaseServices
{
/// <summary>
    /// Database services for Oracle .NET provider.
    /// Service name for configuration file: Oracle
    /// </summary>
    public sealed class OracleDatabaseService : IDatabaseService
    {
        private DbConnection _connection;
        private DbTransaction _transaction;
        
        private DbCommand _assemblySelect;
        private DbCommand _assemblyInsert;
        private DbCommand _versionSelect;
        private DbCommand _versionInsert;
        private DbCommand _stepSelect;
        private DbCommand _stepInsert;
        private DbCommand _stepExecutedQuery;

        public OracleDatabaseService()
        {
        }

        private OracleDatabaseService(string connectionString)
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

        public bool SequenceExists(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(@"name");

            string[] restrictions = { null, name };

            DataTable schema = Connection.GetSchema("Sequences", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
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

            string[] restrictions = { null, tableName};
            
            DataTable schema = Connection.GetSchema("Tables", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }
        public bool TriggerExists(string triggerName)
        {
            if (String.IsNullOrEmpty(triggerName))
                throw new ArgumentNullException(@"triggerName");

            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture, @"select count(*) from dba_triggers where trigger_name = '{0}'", triggerName);

            bool exists = (Convert.ToInt64(cmd.ExecuteScalar(), CultureInfo.InvariantCulture) != 0);

            return exists;
        }
        public bool ViewExists(string viewName)
        {
            if (String.IsNullOrEmpty(viewName))
                throw new ArgumentNullException("viewName");

            string[] restrictions = { null, viewName };

            DataTable schema = Connection.GetSchema("Views", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }
        public bool IndexExists(string indexName, string table)
        {
            if (String.IsNullOrEmpty(indexName))
                throw new ArgumentNullException("indexName");

            string[] restrictions = { null, indexName, null};

            DataTable schema = Connection.GetSchema("Indexes", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }
        public bool ForeignKeyExists(string foreignKeyName, string table)
        {
            if (String.IsNullOrEmpty(foreignKeyName))
                throw new ArgumentNullException("foreignKeyName");

            string[] restrictions = { null, table, foreignKeyName};

            DataTable schema = Connection.GetSchema("ForeignKeys", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }
        public bool PrimaryKeyExists(string primaryKeyName, string table)
        {
            return IndexExists(primaryKeyName, table);
        }
        public string Name
        {
            get { return @"Oracle"; }
        }

        public bool IsUpdateStepExecuted(string assemblyName, string version, int stepNumber)
        {
            _stepExecutedQuery.Parameters[0].Value = assemblyName;
            _stepExecutedQuery.Parameters[1].Value = version;
            _stepExecutedQuery.Parameters[2].Value = stepNumber;

            long count = Convert.ToInt64(_stepExecutedQuery.ExecuteScalar(), CultureInfo.InvariantCulture);

            bool result = (count > 0);

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
            object v = _assemblySelect.ExecuteScalar();
            long? assemblyId = null; 
            
            if (v != null)
                assemblyId = Convert.ToInt64(v, CultureInfo.InvariantCulture);

            if (!assemblyId.HasValue)
            {
                _assemblyInsert.Parameters[0].Value = assemblyName;
                _assemblyInsert.ExecuteScalar();
                assemblyId = Convert.ToInt64(_assemblyInsert.Parameters[1].Value, CultureInfo.InvariantCulture);
            }

            _versionSelect.Parameters[0].Value = assemblyId.Value;
            _versionSelect.Parameters[1].Value = version;

            v = _versionSelect.ExecuteScalar();

            long? versionId = null;
            
            if (v != null)
                versionId = Convert.ToInt64(v, CultureInfo.InvariantCulture);

            if (!versionId.HasValue)
            {
                _versionInsert.Parameters[0].Value = assemblyId.Value;
                _versionInsert.Parameters[1].Value = version;
                _versionInsert.ExecuteScalar();
                versionId = Convert.ToInt64(_versionInsert.Parameters[2].Value, CultureInfo.InvariantCulture);
            }

            _stepSelect.Parameters[0].Value = versionId.Value;
            _stepSelect.Parameters[1].Value = stepNumber;
            v = _stepSelect.ExecuteScalar();
            
            if (v == null)
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

        public bool StoredProcedureExists(string procedureName)
        {
            if (String.IsNullOrEmpty(procedureName))
                throw new ArgumentNullException("procedureName");

            string[] restrictions = { null, procedureName };

            DataTable schema = Connection.GetSchema("Procedures", restrictions);

            bool exists = (schema.Rows.Count != 0);

            return exists;
        }

        public IDatabaseService CloneForConnectionString(string connectionString)
        {
            return new OracleDatabaseService(connectionString);
        }

        public Stream DatabaseSetupXml
        {
            get { return Assembly.GetExecutingAssembly().GetManifestResourceStream(@"DbKeeperNet.Engine.Extensions.DatabaseServices.OracleDatabaseServiceInstall.xml"); }
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
                case "ORACLE":
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

        #region Private members
        private void SetupDbCommands()
        {
            SetupAssemblyDbCommands();
            SetupVersionDbCommands();
            SetupStepDbCommands();

            if (_stepExecutedQuery == null)
            {
                _stepExecutedQuery = Connection.CreateCommand();
                _stepExecutedQuery.CommandText = @"select count(version)  from dbkeepernet_assembly asm
                    , dbkeepernet_version ver , dbkeepernet_step step 
		            where asm.id = ver.dbkeepernet_assembly_id and ver.id = step.dbkeepernet_version_id and step.step = :step 
                    and ver.version = :version and 
                    asm.assembly = :assembly";

                DbParameter assembly = _stepExecutedQuery.CreateParameter();
                assembly.ParameterName = ":assembly";
                DbParameter version = _stepExecutedQuery.CreateParameter();
                version.ParameterName = ":version";
                DbParameter step = _stepExecutedQuery.CreateParameter();
                step.ParameterName = ":step";

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
                _assemblySelect.CommandType = CommandType.Text;

                DbParameter assembly = _assemblySelect.CreateParameter();
                assembly.ParameterName = ":assembly";

                _assemblySelect.Parameters.Add(assembly);
            }
            if (_assemblyInsert == null)
            {
                _assemblyInsert = Connection.CreateCommand();
                _assemblyInsert.CommandText = "insert into dbkeepernet_assembly(assembly, created) values(:assembly, sysdate) returning id into :id";

                DbParameter assembly = _assemblySelect.CreateParameter();
                assembly.ParameterName = ":assembly";
                DbParameter assemblyId = _assemblySelect.CreateParameter();
                _assemblyInsert.Parameters.Add(assembly);

                assemblyId.ParameterName = ":id";
                assemblyId.Direction = ParameterDirection.Output;
                assemblyId.DbType = DbType.Int64;

                _assemblyInsert.Parameters.Add(assemblyId);

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
                _versionInsert.CommandText = "insert into dbkeepernet_version(dbkeepernet_assembly_id, version, created) values(:assemblyId, :version, sysdate) returning id into :id";

                DbParameter assemblyId = _versionInsert.CreateParameter();
                assemblyId.ParameterName = ":assemblyId";

                DbParameter version = _versionInsert.CreateParameter();
                version.ParameterName = ":version";

                _versionInsert.Parameters.Add(assemblyId);
                _versionInsert.Parameters.Add(version);

                DbParameter versionId = _versionInsert.CreateParameter();

                versionId.ParameterName = ":id";
                versionId.Direction = ParameterDirection.Output;
                versionId.DbType = DbType.Int64;

                _versionInsert.Parameters.Add(versionId);

            }
        }
        private void SetupStepDbCommands()
        {
            if (_stepSelect == null)
            {
                _stepSelect = Connection.CreateCommand();
                _stepSelect.CommandText = "select id from dbkeepernet_step where dbkeepernet_version_id = :versionId and step = :step";

                DbParameter versionId = _stepSelect.CreateParameter();
                versionId.ParameterName = ":versionId";

                DbParameter step = _stepSelect.CreateParameter();
                step.ParameterName = ":step";

                _stepSelect.Parameters.Add(versionId);
                _stepSelect.Parameters.Add(step);
            }
            if (_stepInsert == null)
            {
                _stepInsert = Connection.CreateCommand();
                _stepInsert.CommandText = "insert into dbkeepernet_step(dbkeepernet_version_id, step, created) values(:versionId, :step, sysdate)";

                DbParameter versionId = _assemblySelect.CreateParameter();
                versionId.ParameterName = ":versionId";

                DbParameter step = _assemblySelect.CreateParameter();
                step.ParameterName = ":step";

                _stepInsert.Parameters.Add(versionId);
                _stepInsert.Parameters.Add(step);
            }
        }

        #endregion
    }
}

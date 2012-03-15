using System;
using System.Data.Common;
using System.IO;
using System.Configuration;
using System.Data;
using System.Reflection;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.DatabaseServices
{
    /// <summary>
    /// Database services for MySQL servers connected via MySQLNetConnector.
    /// Service name for configuration file: MySqlNet
    /// </summary>
    public class MySqlNetConnectorDatabaseService : DisposableObject, IDatabaseService
    {
        #region Private member variables
        private DbConnection _connection;
        private DbTransaction _transaction;
        private DbProviderFactory _factory;
        private DbCommand _assemblySelect;
        private DbCommand _assemblyInsert;
        private DbCommand _versionSelect;
        private DbCommand _versionInsert;
        private DbCommand _stepSelect;
        private DbCommand _stepInsert;
        private DbCommand _stepExecutedQuery; 
        #endregion

        #region Constructors
        public MySqlNetConnectorDatabaseService()
        {
        }
        /// <summary>
        /// Initialize instanace of database service using given database
        /// connection
        /// </summary>
        /// <remarks>Database connection must be opened and is released automatically</remarks>
        /// <param name="databaseConnection"></param>
        public MySqlNetConnectorDatabaseService(DbConnection databaseConnection)
        {
            if (databaseConnection == null)
                throw new ArgumentNullException(@"databaseConnection");

            if (databaseConnection.State != ConnectionState.Open)
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, DatabaseServiceMessages.ConnectionMustBeOpened, databaseConnection.State));

            _connection = databaseConnection;
        }
        private MySqlNetConnectorDatabaseService(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            ConnectionStringSettings connectString = ConfigurationManager.ConnectionStrings[connectionString];

            if (connectString == null)
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, DatabaseServiceMessages.ConnectionStringNotFound, connectionString));

            _factory = DbProviderFactories.GetFactory(connectString.ProviderName);
            if (_factory == null)
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, DatabaseServiceMessages.ConnectionStringNotFound, connectString.ProviderName));

            _connection = _factory.CreateConnection();
            _connection.ConnectionString = connectString.ConnectionString;
            _connection.Open();

            SetupDbCommands();
        } 
        #endregion

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
            if (String.IsNullOrEmpty(table))
                throw new ArgumentNullException("table");

            DataTable schema = Connection.GetSchema("Indexes");

            bool exists = false;

            foreach (DataRow row in schema.Rows)
            {
                if (indexName.Equals((string)row[2], StringComparison.OrdinalIgnoreCase)
                    && table.Equals((string)row[3], StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }
        public bool PrimaryKeyExists(string primaryKeyName, string table)
        {
            if (String.IsNullOrEmpty(primaryKeyName))
                throw new ArgumentNullException("primaryKeyName");
            if (String.IsNullOrEmpty(table))
                throw new ArgumentNullException("table");

            DataTable schema = Connection.GetSchema("Indexes");

            bool exists = false;

            foreach (DataRow row in schema.Rows)
            {
                if ("PRIMARY".Equals((string)row[2], StringComparison.OrdinalIgnoreCase)
                    && table.Equals((string)row[3], StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }
        public bool TriggerExists(string triggerName)
        {
            throw new NotSupportedException();
        }
        public bool ForeignKeyExists(string foreignKeyName, string table)
        {
            if (String.IsNullOrEmpty(foreignKeyName))
                throw new ArgumentNullException("foreignKeyName");
            if (String.IsNullOrEmpty(table))
                throw new ArgumentNullException("table");
            
            DataTable schema = Connection.GetSchema("Indexes");

            bool exists = false;

            foreach (DataRow row in schema.Rows)
            {
                if (foreignKeyName.Equals((string)row[2], StringComparison.OrdinalIgnoreCase)
                    && table.Equals((string)row[3], StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }
        public string Name
        {
            get { return @"MySqlNet"; }
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
            int? assemblyId = (int?)_assemblySelect.ExecuteScalar();
            
            if (!assemblyId.HasValue)
            {
                _assemblyInsert.Parameters[0].Value = assemblyName;
                assemblyId = Convert.ToInt32(_assemblyInsert.ExecuteScalar(), CultureInfo.InvariantCulture);
            }

            _versionSelect.Parameters[0].Value = assemblyId.Value;
            _versionSelect.Parameters[1].Value = version;
            int? versionId = (int?)_versionSelect.ExecuteScalar();

            if (!versionId.HasValue)
            {
                _versionInsert.Parameters[0].Value = assemblyId.Value;
                _versionInsert.Parameters[1].Value = version;
                versionId = Convert.ToInt32(_versionInsert.ExecuteScalar(), CultureInfo.InvariantCulture);
            }

            _stepSelect.Parameters[0].Value = versionId.Value;
            _stepSelect.Parameters[1].Value = stepNumber;
            int? stepId = (int?)_stepSelect.ExecuteScalar();

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

        public bool StoredProcedureExists(string procedureName)
        {
            throw new NotSupportedException();
        }

        public IDatabaseService CloneForConnectionString(string connectionString)
        {
            return new MySqlNetConnectorDatabaseService(connectionString);
        }

        public Stream DatabaseSetupXml
        {
            get { return Assembly.GetExecutingAssembly().GetManifestResourceStream(@"DbKeeperNet.Engine.Extensions.DatabaseServices.MySqlNetConnectorDatabaseServiceInstall.xml"); }
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
                case "MYSQL":
                    status = true;
                    break;
            }

            return status;
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
                        _connection.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

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
                _assemblySelect.CommandText = "select id from dbkeepernet_assembly where assembly = @assembly";

                DbParameter assembly = _assemblySelect.CreateParameter();
                assembly.ParameterName = "@assembly";

                _assemblySelect.Parameters.Add(assembly);
            }
            if (_assemblyInsert == null)
            {
                _assemblyInsert = Connection.CreateCommand();
                _assemblyInsert.CommandText = "insert into dbkeepernet_assembly(assembly, created) values(@assembly, now()); select last_insert_id()";

                DbParameter assembly = _assemblyInsert.CreateParameter();
                assembly.ParameterName = "@assembly";

                _assemblyInsert.Parameters.Add(assembly);
            }
        }
        private void SetupVersionDbCommands()
        {
            if (_versionSelect == null)
            {
                _versionSelect = Connection.CreateCommand();
                _versionSelect.CommandText = "select id from dbkeepernet_version where dbkeepernet_assembly_id = @assemblyId and version = @version";

                DbParameter assemblyId = _versionSelect.CreateParameter();
                assemblyId.ParameterName = "@assemblyId";

                DbParameter version = _versionSelect.CreateParameter();
                version.ParameterName = "@version";

                _versionSelect.Parameters.Add(assemblyId);
                _versionSelect.Parameters.Add(version);
            }
            if (_versionInsert == null)
            {
                _versionInsert = Connection.CreateCommand();
                _versionInsert.CommandText = "insert into dbkeepernet_version(dbkeepernet_assembly_id, version, created) values(@assemblyId, @version, now()); select last_insert_id() ";

                DbParameter assemblyId = _versionInsert.CreateParameter();
                assemblyId.ParameterName = "@assemblyId";

                DbParameter version = _versionInsert.CreateParameter();
                version.ParameterName = "@version";

                _versionInsert.Parameters.Add(assemblyId);
                _versionInsert.Parameters.Add(version);
            }
        }
        private void SetupStepDbCommands()
        {
            if (_stepSelect == null)
            {
                _stepSelect = Connection.CreateCommand();
                _stepSelect.CommandText = "select id from dbkeepernet_step where dbkeepernet_version_id = @versionId and step = @step";

                DbParameter versionId = _stepSelect.CreateParameter();
                versionId.ParameterName = "@versionId";

                DbParameter step = _stepSelect.CreateParameter();
                step.ParameterName = "@step";

                _stepSelect.Parameters.Add(versionId);
                _stepSelect.Parameters.Add(step);
            }
            if (_stepInsert == null)
            {
                _stepInsert = Connection.CreateCommand();
                _stepInsert.CommandText = "insert into dbkeepernet_step(dbkeepernet_version_id, step, created) values(@versionId, @step, now())";

                DbParameter versionId = _stepInsert.CreateParameter();
                versionId.ParameterName = "@versionId";

                DbParameter step = _stepInsert.CreateParameter();
                step.ParameterName = "@step";

                _stepInsert.Parameters.Add(versionId);
                _stepInsert.Parameters.Add(step);
            }
        }
        #endregion
    }
}

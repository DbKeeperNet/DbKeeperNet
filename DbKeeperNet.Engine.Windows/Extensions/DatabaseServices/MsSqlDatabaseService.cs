using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection;
using DbKeeperNet.Engine.Extensions.Preconditions;
using DbKeeperNet.Engine.Resources;
using DbType = System.Data.DbType;

namespace DbKeeperNet.Engine.Windows.Extensions.DatabaseServices
{
    /// <summary>
    /// Database services for MsSQL server 2000 or higher.
    /// </summary>
    /// <remarks>
    /// Supported preconditions:
    /// <list type="bullet">
    /// <item><see cref="DbForeignKeyNotFound"/></item>
    /// <item><see cref="DbIndexNotFound"/></item>
    /// <item><see cref="DbPrimaryKeyNotFound"/></item>
    /// <item><see cref="DbProcedureNotFound"/></item>
    /// <item><see cref="DbTableNotFound"/></item>
    /// <item><see cref="DbTriggerNotFound"/></item>
    /// <item><see cref="DbViewNotFound"/></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// Mapping of connection string to database service in App.Config file:
    /// <code>
    /// <![CDATA[
    /// <add connectString="default" databaseService="MsSql" />
    /// ]]>
    /// </code>
    /// </example>
    /// 
    /// <example>
    /// Usage in database upgrade script for conditional behavior - case insensitive comparison of value <c>MSSQL</c>.
    /// For details see <see cref="IsDbType"/>.
    /// <code>
    /// <![CDATA[
    /// <AlternativeStatement DbType="MsSql">
    /// SELECT 1
    /// </AlternativeStatement>
    /// ]]>
    /// </code>
    /// </example>
    public sealed class MsSqlDatabaseService : DisposableObject, IDatabaseService
    {
        #region Private member variables
        private DbConnection _connection;
        private DbTransaction _transaction;
        #endregion

        #region Constructors
        public MsSqlDatabaseService()
        {
        }
        /// <summary>
        /// Initialize instanace of database service using given database
        /// connection
        /// </summary>
        /// <remarks>Database connection must be opened and is released automatically</remarks>
        /// <param name="databaseConnection"></param>
        public MsSqlDatabaseService(SqlConnection databaseConnection)
        {
            if (databaseConnection == null)
                throw new ArgumentNullException(@"databaseConnection");

            if (databaseConnection.State != ConnectionState.Open)
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, DatabaseServiceMessages.ConnectionMustBeOpened, databaseConnection.State));

            _connection = databaseConnection;
        }

        private MsSqlDatabaseService(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            ConnectionStringSettings connectString = ConfigurationManager.ConnectionStrings[connectionString];

            if (connectString == null)
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, DatabaseServiceMessages.ConnectionStringNotFound, connectionString));

            _connection = DbProviderFactories.GetFactory(connectString.ProviderName).CreateConnection();
            _connection.ConnectionString = connectString.ConnectionString;
            _connection.Open();
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
            cmd.CommandText = @"select count(*) from sysobjects A where A.xtype='TR' and A.name = @triggerName";
            
            var parameter = cmd.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = @"@triggerName";
            parameter.Value = triggerName;
            cmd.Parameters.Add(parameter);

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
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
            get { return typeof(MsSqlDatabaseService).GetTypeInfo().Assembly.GetManifestResourceStream(@"DbKeeperNet.Engine.Windows.Extensions.DatabaseServices.MsSqlDatabaseServiceInstall.xml"); }
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

        /// <summary>
        /// Checks whether the given <paramref name="dbTypeName"/>
        /// is supported by this database service.
        /// </summary>
        /// <param name="dbTypeName">Database type as it is used in XML update definition.</param>
        /// <returns><c>true</c> - this database service supports the given database type, <c>false</c> - doesn't support.</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <listheader>Following <paramref name="dbTypeName"/> values are case insensitively recognized as database type Microsoft SQL Server:</listheader>
        /// <item>MSSQL</item>
        /// </list>
        /// </remarks>
        public bool IsDbType(string dbTypeName)
        {
            bool status = false;

            if (dbTypeName != null)
            {
                switch (dbTypeName.ToUpperInvariant())
                {
                    case @"MSSQL":
                        status = true;
                        break;
                }
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
                        // we must release database connection
                        _connection.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }


        }
    }
}
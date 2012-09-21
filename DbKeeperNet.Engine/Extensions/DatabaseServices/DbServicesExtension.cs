using System;

namespace DbKeeperNet.Engine.Extensions.DatabaseServices
{
    /// <summary>
    /// Registration entry point for built-in database services
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>Registers support for following databases:</listheader>
    /// <item><see cref="MsSqlDatabaseService">Microsoft SQL Server</see></item>
    /// <item><see cref="MySqlNetConnectorDatabaseService">MySQL</see></item>
    /// <item><see cref="OracleDatabaseService">Oracle</see></item>
    /// <item><see cref="PgSqlDatabaseService">PostgreSQL</see></item>
    /// <item><see cref="SQLiteDatabaseService">SQLite</see></item>
    /// <item><see cref="FirebirdDatabaseService">Firebird</see></item>
    /// </list>
    ///
    /// All providers implement interface <see cref="IDatabaseService"/>
    /// </remarks>
    public sealed class DbServicesExtension : IExtension
    {
        #region IExtension Members

        public void Initialize(IUpdateContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.RegisterDatabaseService(new MsSqlDatabaseService());
            context.RegisterDatabaseService(new MySqlNetConnectorDatabaseService());
            context.RegisterDatabaseService(new OracleDatabaseService());

            context.RegisterDatabaseService(new PgSqlDatabaseService());
            context.RegisterDatabaseService(new SQLiteDatabaseService());
            context.RegisterDatabaseService(new FirebirdDatabaseService());
        }

        #endregion
    }
}

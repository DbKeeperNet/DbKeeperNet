using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.DefaultCheckers;
using DbKeeperNet.Extensions.SQLite.Checkers;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace DbKeeperNet.Extensions.SQLite
{
    public static class DbKeeperNetBuilderExtensions
    {
        public static IDbKeeperNetBuilder UseSQLite(this IDbKeeperNetBuilder configuration, string connectionString)
        {
            configuration.Services
                .AddScoped<IDatabaseService>(c => new SQLiteDatabaseService(connectionString))
                .AddTransient(c => (IDatabaseService<SqliteConnection>)c.GetService<IDatabaseService>())
                .AddScoped<IDatabaseServiceTransactionProvider, DatabaseServiceTransactionProvider<SqliteTransaction>>()
                .AddTransient(c => (IDatabaseServiceTransactionProvider<SqliteTransaction>)c.GetService<IDatabaseServiceTransactionProvider>())

                .AddTransient<IDatabaseServiceInstaller, SQLiteDatabaseServiceInstaller>()

                .AddTransient<IUpdateStepExecutedMarker, SQLiteUpdateStepExecutedMarker>()
                .AddTransient<IUpdateStepExecutedChecker, SQLiteUpdateStepExecutedChecker>()

                .AddTransient<IDatabaseLock, SQLiteDatabaseLock>()

                .AddTransient<IDatabaseServiceCommandHandler, GenericDatabaseServiceCommandHandler>()

                .AddTransient<IDatabaseServiceForeignKeyChecker, SQLiteForeignKeyChecker>()
                .AddTransient<IDatabaseServicePrimaryKeyChecker, SQLitePrimaryKeyChecker>()
                .AddTransient<IDatabaseServiceIndexChecker, SQLiteIndexChecker>()
                .AddTransient<IDatabaseServiceTableChecker, SQLiteTableChecker>()
                .AddTransient<IDatabaseServiceViewChecker, SQLiteViewChecker>()
                .AddTransient<IDatabaseServiceTriggerChecker, ExistsTriggerChecker>()
                .AddTransient<IDatabaseServiceStoredProcedureChecker, ExistsStoredProcedureChecker>()
                ;

            return configuration;
        }
    }

}

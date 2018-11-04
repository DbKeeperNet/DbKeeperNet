using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.DefaultCheckers;
using DbKeeperNet.Extensions.Pgsql.Checkers;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace DbKeeperNet.Extensions.Pgsql
{
    public static class DbKeeperNetBuilderExtensions
    {
        public static IDbKeeperNetBuilder UsePgsql(this IDbKeeperNetBuilder configuration, string connectionString)
        {
            configuration.Services
                .AddScoped<IDatabaseService>(c => new PgsqlDatabaseService(connectionString))
                .AddTransient(c => (IDatabaseService<NpgsqlConnection>)c.GetService<IDatabaseService>())

                .AddScoped<IDatabaseServiceTransactionProvider, DatabaseServiceTransactionProvider<NpgsqlTransaction>>()
                .AddTransient(c => (IDatabaseServiceTransactionProvider<NpgsqlTransaction>)c.GetService<IDatabaseServiceTransactionProvider>())

                .AddTransient<IDatabaseLock, PgSqlDatabaseLock>()

                .AddTransient<IUpdateStepExecutedChecker, PgsqlUpdateStepExecutedChecker>()
                .AddTransient<IUpdateStepExecutedMarker, PgsqlUpdateStepExecutedMarker>()

                .AddTransient<IDatabaseServiceInstaller, PgsqlDatabaseServiceInstaller>()

                .AddTransient<IDatabaseServiceCommandHandler, GenericDatabaseServiceCommandHandler>()

                .AddTransient<IDatabaseServiceForeignKeyChecker, PgsqlDatabaseServiceForeignKeyChecker>()
                .AddTransient<IDatabaseServiceIndexChecker, PgsqlDatabaseServiceIndexChecker>()
                .AddTransient<IDatabaseServicePrimaryKeyChecker, PgsqlDatabaseServiceIndexChecker>()
                .AddTransient<IDatabaseServiceStoredProcedureChecker, ExistsStoredProcedureChecker>()
                .AddTransient<IDatabaseServiceTableChecker, PgsqlDatabaseServiceTableChecker>()
                .AddTransient<IDatabaseServiceViewChecker, PgsqlDatabaseServiceViewChecker>()

                .AddTransient< IDatabaseServiceTriggerChecker, ExistsTriggerChecker>()
                ;

            return configuration;
        }
    }

}

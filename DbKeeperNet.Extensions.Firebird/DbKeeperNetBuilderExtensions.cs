using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.DefaultCheckers;
using DbKeeperNet.Extensions.Firebird.Checkers;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.DependencyInjection;

namespace DbKeeperNet.Extensions.Firebird
{
    public static class DbKeeperNetBuilderExtensions
    {
        public static IDbKeeperNetBuilder UseFirebird(this IDbKeeperNetBuilder configuration, string connectionString)
        {
            configuration.Services
                .AddScoped<IDatabaseService>(c => new FirebirdDatabaseService(connectionString))
                .AddTransient(c => (IDatabaseService<FbConnection>)c.GetService<IDatabaseService>())

                .AddScoped<IDatabaseServiceTransactionProvider, DatabaseServiceTransactionProvider<FbTransaction>>()
                .AddTransient(c => (IDatabaseServiceTransactionProvider<FbTransaction>)c.GetService<IDatabaseServiceTransactionProvider>())
                
                .AddTransient<IDatabaseServiceInstaller, FirebirdDatabaseServiceInstaller>()

                .AddTransient<IDatabaseServiceCommandHandler, GenericDatabaseServiceCommandHandler>()

                .AddTransient<IUpdateStepExecutedChecker, FirebirdUpdateStepExecutedChecker>()
                .AddTransient<IUpdateStepExecutedMarker, FirebirdUpdateStepExecutedMarker>()

                // TODO: review
                // .AddTransient<IDatabaseServiceForeignKeyChecker, FirebirdDatabaseServiceForeignKeyChecker>()
                .AddTransient<IDatabaseServiceForeignKeyChecker, ExistsForeignKeyChecker>()
                .AddTransient<IDatabaseServiceIndexChecker, FirebirdDatabaseServiceIndexChecker>()
                .AddTransient<IDatabaseServicePrimaryKeyChecker, FirebirdDatabaseServiceIndexChecker>()
                .AddTransient<IDatabaseServiceStoredProcedureChecker, ExistsStoredProcedureChecker>()
                .AddTransient<IDatabaseServiceTableChecker, FirebirdDatabaseServiceTableChecker>()
                .AddTransient<IDatabaseServiceViewChecker, FirebirdDatabaseServiceViewChecker>()

                .AddTransient<IDatabaseServiceTriggerChecker, FirebirdDatabaseServiceTriggerChecker>()

                ;

            configuration
                .AddScriptSplitter<SqlScriptSplitter>();

            return configuration;
        }
    }

}

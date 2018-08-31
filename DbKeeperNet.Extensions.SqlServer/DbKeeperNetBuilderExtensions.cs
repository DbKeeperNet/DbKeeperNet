using System.Data.SqlClient;
using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.DefaultCheckers;
using DbKeeperNet.Extensions.SqlServer.Checkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Extensions.SqlServer
{
    public static class DbKeeperNetBuilderExtensions
    {

        public static IDbKeeperNetBuilder UseSqlServer(this IDbKeeperNetBuilder builder, string connectionString)
        {
            builder.Services
                .AddScoped<IDatabaseService>(d => new MicrosoftSqlDatabaseService(connectionString, d.GetService<ILogger<MicrosoftSqlDatabaseService>>()))
                .AddTransient(c => (IDatabaseService<SqlConnection>)c.GetService<IDatabaseService>())
                .AddTransient<IDatabaseServiceInstaller, MicrosoftSqlDatabaseServiceInstaller>()
                
                .AddScoped<IDatabaseServiceTransactionProvider, DatabaseServiceTransactionProvider<SqlTransaction>>()
                .AddTransient(c => (IDatabaseServiceTransactionProvider<SqlTransaction>)c.GetService<IDatabaseServiceTransactionProvider>())

                .AddTransient<IDatabaseServiceCommandHandler, GenericDatabaseServiceCommandHandler>()

                .AddTransient<IDatabaseServiceForeignKeyChecker, MicrosoftSqlDatabaseServiceForeignKeyChecker>()
                .AddTransient<IDatabaseServicePrimaryKeyChecker, MicrosoftSqlDatabaseServiceIndexChecker>()
                .AddTransient<IDatabaseServiceIndexChecker, MicrosoftSqlDatabaseServiceIndexChecker>()
                .AddTransient<IDatabaseServiceStoredProcedureChecker, MicrosoftSqlDatabaseServiceStoredProcedureChecker>()
                .AddTransient<IDatabaseServiceTableChecker, MicrosoftSqlDatabaseServiceTableChecker>()
                .AddTransient<IDatabaseServiceViewChecker, MicrosoftSqlDatabaseServiceViewChecker>()

                .AddTransient<IUpdateStepExecutedChecker, MicrosoftSqlDatabaseServiceUpdateStepExecutedChecker>()
                .AddTransient<IUpdateStepExecutedMarker, MicrosoftSqlDatabaseUpdateStepExecutedMarker>()
                
                .AddTransient<IDatabaseServiceTriggerChecker, ExistsTriggerChecker>()

                ;

            return builder;
        }
    }
}
﻿using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.DefaultCheckers;
using DbKeeperNet.Extensions.Mysql.Checkers;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace DbKeeperNet.Extensions.Mysql
{
    public static class DbKeeperNetBuilderExtensions
    {
        public static IDbKeeperNetBuilder UseMysql(this IDbKeeperNetBuilder configuration, string connectionString)
        {
            return UseMysql(configuration, new StaticConnectionStringProvider(connectionString));
        }

        public static IDbKeeperNetBuilder UseMysql(this IDbKeeperNetBuilder configuration, IConnectionStringProvider connectionStringProvider)
        {
            configuration.Services
                .AddTransient(c => connectionStringProvider)

                .AddScoped<IDatabaseService, MysqlDatabaseService>()
                .AddTransient(c => (IDatabaseService<MySqlConnection>)c.GetService<IDatabaseService>())

                .AddScoped<IDatabaseServiceTransactionProvider, DatabaseServiceTransactionProvider<MySqlTransaction>>()
                .AddTransient(c => (IDatabaseServiceTransactionProvider<MySqlTransaction>)c.GetService<IDatabaseServiceTransactionProvider>())

                .AddTransient<IDatabaseLock, MySqlDatabaseLock>()

                .AddTransient<IDatabaseServiceInstaller, MysqlDatabaseServiceInstaller>()

                .AddTransient<IDatabaseServiceIndexChecker, MySqlDatabaseServiceIndexChecker>()

                .AddTransient<IDatabaseServiceCommandHandler, GenericDatabaseServiceCommandHandler>()

                .AddTransient<IUpdateStepExecutedChecker, MySqlUpdateStepExecutedChecker>()
                .AddTransient<IUpdateStepExecutedMarker, MySqlUpdateStepExecutedMarker>()

                .AddTransient<IDatabaseServiceForeignKeyChecker, MySqlDatabaseServiceForeignKeyChecker>()
                .AddTransient<IDatabaseServicePrimaryKeyChecker, MySqlDatabaseServicePrimaryKeyChecker>()
                .AddTransient<IDatabaseServiceIndexChecker, MySqlDatabaseServiceIndexChecker>()
                .AddTransient<IDatabaseServiceStoredProcedureChecker, ExistsStoredProcedureChecker>()
                .AddTransient<IDatabaseServiceTableChecker, MySqlDatabaseServiceTableChecker>()
                .AddTransient<IDatabaseServiceViewChecker, MySqlDatabaseServiceViewChecker>()

                .AddTransient<IDatabaseServiceTriggerChecker, ExistsTriggerChecker>()
                ;

            return configuration;
        }
    }

}

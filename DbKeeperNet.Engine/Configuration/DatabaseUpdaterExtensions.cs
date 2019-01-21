using System;
using System.Xml;
using DbKeeperNet.Engine.Preconditions;
using DbKeeperNet.Engine.ScriptProviders;
using DbKeeperNet.Engine.UpdateStepHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Engine.Configuration
{
    public static class DbKeeperNetBuilderExtensions
    {
        /// <summary>
        /// Initialize DbKeeperNet feature in the IoC container configuration
        /// </summary>
        /// <param name="serviceCollection">Service collection</param>
        /// <param name="configurationCallback">Additional configuration delegate</param>
        /// <returns></returns>
        public static IServiceCollection AddDbKeeperNet(this IServiceCollection serviceCollection, Action<IDbKeeperNetBuilder> configurationCallback)
        {
            var configuration = new DbKeeperNetBuilder(serviceCollection);

            serviceCollection

                .AddTransient<IPreconditionHandler, ForceExecutionPrecondition>()
                .AddTransient<IPreconditionHandler, ForeignKeyNotFoundPrecondition>()
                .AddTransient<IPreconditionHandler, IndexNotFoundPrecondition>()
                .AddTransient<IPreconditionHandler, PrimaryKeyNotFoundPrecondition>()
                .AddTransient<IPreconditionHandler, StoredProcedureNotFoundPrecondition>()
                .AddTransient<IPreconditionHandler, TableNotFoundPrecondition>()
                .AddTransient<IPreconditionHandler, TriggerNotFoundPrecondition>()
                .AddTransient<IPreconditionHandler, UpdateStepNotExecutedPrecondition>()
                .AddTransient<IPreconditionHandler, ViewNotFoundPrecondition>()
                .AddTransient<IPreconditionHandler, DbTypePrecondition>()
                
                .AddTransient<IUpdateStepHandler, DbCommandStepHandler>()
                .AddTransient<IUpdateStepHandler, CustomStepHandler>()

                .AddTransient<IUpdateStepService, UpdateStepService>()
                .AddTransient<IDatabaseUpdater, DatabaseUpdater>()
                .AddTransient<IPreconditionService, PreconditionService>()
                .AddTransient<IScriptDeserializer, ScriptDeserializer>()
                .AddTransient<IDatabaseLockService, DatabaseLockService>()

                .AddScoped<IUpdateScriptManager, UpdateScriptManager>()
                ;

            configuration
                .AddScriptSplitter<ScriptSplitterDoingNothing>();

            configurationCallback(configuration);

            var schemaResource = typeof(DbKeeperNetBuilderExtensions).Assembly.GetManifestResourceStream(@"DbKeeperNet.Engine.Resources.Updates-1.0.xsd");
            
            configuration
                .AddSchema(@"http://code.google.com/p/dbkeepernet/Updates-1.0.xsd", XmlReader.Create(schemaResource) /*, typeof(Updates) */);

            foreach (var script in configuration.Scripts)
            {
                serviceCollection.AddTransient<IScriptProviderService>(script);
            }

            serviceCollection
                .AddTransient<Lazy<IDatabaseUpdater>>(f => new Lazy<IDatabaseUpdater>(() => f.GetService<IDatabaseUpdater>()))
                .AddTransient<IScriptSchemaProvider>(f => new ScriptSchemaProvider(configuration.Schemas))
                .AddTransient<IUpdateScriptManager>(f =>
                {
                    var s = f.GetServices<IScriptProviderService>();
                    return new UpdateScriptManager(s);
                })
                .AddTransient<ISqlScriptSplitter>(f => configuration.ScriptSplitter)
                ;

            return serviceCollection;
        }

        /// <summary>
        /// Add a new database upgrade script contained in the embedded resource
        /// </summary>
        /// <param name="builder">Configuration builder</param>
        /// <param name="resourceName">Resource name in form of <c>PathToResource.xml,AssemblyName</c></param>
        /// <returns>The configuration builder for fluent syntax</returns>
        public static IDbKeeperNetBuilder AddEmbeddedResourceScript(this IDbKeeperNetBuilder builder, string resourceName)
        {
            builder.AddScript(c => new EmbeddedResourceUpdateScriptProvider(resourceName));
            return builder;
        }

        /// <summary>
        /// Add a new database upgrade script contained in a disk file
        /// </summary>
        /// <param name="builder">Configuration builder</param>
        /// <param name="path">File path</param>
        /// <returns>The configuration builder for fluent syntax</returns>
        public static IDbKeeperNetBuilder AddDiskFileScript(this IDbKeeperNetBuilder builder, string path)
        {
            builder.AddScript(s =>
            {
                var logger = s.GetService<ILogger<DiskFileUpdateScriptProvider>>();
                return new DiskFileUpdateScriptProvider(logger, path);
            });

            return builder;
        }
    }
}
using System;
using System.Xml;
using DbKeeperNet.Engine.Preconditions;
using DbKeeperNet.Engine.ScriptProviders;
using DbKeeperNet.Engine.UpdateStepHandlers;
using Microsoft.Extensions.DependencyInjection;

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
                .AddScoped<IUpdateScriptManager, UpdateScriptManager>()
                ;

            configuration
                .AddScriptSplitter<ScriptSplitterDoingNothing>();

            configurationCallback(configuration);

            var schemaResource = typeof(DbKeeperNetBuilderExtensions).Assembly.GetManifestResourceStream(@"DbKeeperNet.Engine.Resources.Updates-1.0.xsd");
            
            configuration
                .AddSchema(@"http://code.google.com/p/dbkeepernet/Updates-1.0.xsd", XmlReader.Create(schemaResource) /*, typeof(Updates) */);

            serviceCollection
                .AddScoped<IScriptSchemaProvider>(f => new ScriptSchemaProvider(configuration.Schemas))
                .AddScoped<IUpdateScriptManager>(f => new UpdateScriptManager(configuration.Scripts))
                .AddScoped<ISqlScriptSplitter>(f => configuration.ScriptSplitter)
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
            builder.AddScript(new EmbeddedResourceUpdateScriptProvider(resourceName));
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
            builder.AddScript(new DiskFileUpdateScriptProvider(path));
            return builder;
        }
    }
}
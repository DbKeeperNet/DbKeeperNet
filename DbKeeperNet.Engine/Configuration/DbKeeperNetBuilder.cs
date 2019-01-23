using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;

namespace DbKeeperNet.Engine.Configuration
{
    public class DbKeeperNetBuilder : IDbKeeperNetBuilder
    {
        public DbKeeperNetBuilder(IServiceCollection services)
        {
            Services = services;
            Scripts = new List<Func<IServiceProvider, IScriptProviderService>>();
            Schemas = new List<SchemaReference>();
        }

        public IServiceCollection Services { get; }

        public IDbKeeperNetBuilder AddScript(Func<IServiceProvider, IScriptProviderService> scriptCreator)
        {
            Scripts.Add(scriptCreator);

            return this;
        }

        public IDbKeeperNetBuilder AddScript(IScriptProviderService script)
        {
            return this;
        }

        public IDbKeeperNetBuilder AddSchema(string schemaNamespace, Func<XmlReader> schema, params Type[] types)
        {
            Schemas.Add(new SchemaReference(schemaNamespace, schema, types));

            return this;
        }

        public IDbKeeperNetBuilder AddScriptSplitter<T>() where T : ISqlScriptSplitter, new()
        {
            var scriptSplitter = new T();

            if (ScriptSplitter != null)
            {
                ScriptSplitter.SetNext(scriptSplitter);
            }
            else
            {
                ScriptSplitter = scriptSplitter;
            }

            return this;
        }

        public IList<Func<IServiceProvider, IScriptProviderService>> Scripts { get; }

        public IList<SchemaReference> Schemas { get; }

        public ISqlScriptSplitter ScriptSplitter { get; set; }
    }
}
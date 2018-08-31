using System;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;

namespace DbKeeperNet.Engine.Configuration
{
    /// <summary>
    /// An interface for DbKeeperNet configuration builder
    /// </summary>
    public interface IDbKeeperNetBuilder
    {
        /// <summary>
        /// Provides direct access to the IoC container configuration
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Register a new database upgrade (migration) script to be executed
        /// </summary>
        /// <remarks>
        /// Scripts are executed in the order of their registrations
        /// </remarks>
        /// <param name="script">Script provider instance</param>
        /// <returns>The builder for fluent syntax</returns>
        IDbKeeperNetBuilder AddScript(IScriptProviderService script);

        /// <summary>
        /// Registers a new script splitter service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The builder for fluent syntax</returns>
        IDbKeeperNetBuilder AddScriptSplitter<T>() where T : ISqlScriptSplitter, new();

        /// <summary>
        /// Register a new migration script schema extension
        /// </summary>
        /// <param name="schemaNamespace">XML schema extension namespace</param>
        /// <param name="schema">XML schema</param>
        /// <param name="types">List of C# types used for serialization of <paramref name="schema"/></param>
        /// <returns>The builder for fluent syntax</returns>
        IDbKeeperNetBuilder AddSchema(string schemaNamespace, XmlReader schema, params Type[] types);
    }
}
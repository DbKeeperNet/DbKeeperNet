using System;
using System.Xml;

namespace DbKeeperNet.Engine
{
    public class SchemaReference
    {
        public SchemaReference(string schemaNamespace, Func<XmlReader> schema, params Type[] types)
        {
            SchemaNamespace = schemaNamespace;
            Schema = schema;
            Types = types;
        }

        public string SchemaNamespace { get; }

        public Func<XmlReader> Schema { get; }
        public Type[] Types { get; }
    }
}
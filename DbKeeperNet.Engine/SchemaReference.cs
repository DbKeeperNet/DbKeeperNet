using System;
using System.Xml;

namespace DbKeeperNet.Engine
{
    public class SchemaReference
    {
        public SchemaReference(string schemaNamespace, XmlReader schema, params Type[] types)
        {
            SchemaNamespace = schemaNamespace;
            Schema = schema;
            Types = types;
        }

        public string SchemaNamespace { get; }

        public XmlReader Schema { get; }
        public Type[] Types { get; }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DbKeeperNet.Engine
{
    public class ScriptDeserializer : IScriptDeserializer
    {
        private readonly XmlSerializer _xmlSerializer;
        private readonly XmlReaderSettings _settings;

        public ScriptDeserializer(IScriptSchemaProvider scriptSchemaProvider)
        {
            var schemaSet = new XmlSchemaSet();
            var types = new List<Type>();

            foreach (var schema in scriptSchemaProvider.GetSchemas())
            {
                schemaSet.Add(schema.SchemaNamespace, schema.Schema);
                types.AddRange(schema.Types);
            }

            _settings = new XmlReaderSettings();
            _settings.Schemas.Add(schemaSet);
            _settings.IgnoreWhitespace = true;
            _settings.ValidationType = ValidationType.Schema;
            
            _xmlSerializer = new XmlSerializer(typeof(Updates), types.ToArray());
        }

        public Updates GetDeserializedScript(Stream source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            
            using (var xmlReader = XmlReader.Create(source, _settings))
            {
                
                return (Updates)_xmlSerializer.Deserialize(xmlReader);
            }
        }
    }
}
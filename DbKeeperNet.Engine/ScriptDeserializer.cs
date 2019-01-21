using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Engine
{
    public class ScriptDeserializer : IScriptDeserializer
    {
        private readonly ILogger<ScriptDeserializer> _logger;
        private readonly XmlSerializer _xmlSerializer;
        private readonly XmlReaderSettings _settings;

        public ScriptDeserializer(ILogger<ScriptDeserializer> logger, IScriptSchemaProvider scriptSchemaProvider)
        {
            _logger = logger;
            var schemaSet = new XmlSchemaSet();
            var types = new List<Type>();

            foreach (var schema in scriptSchemaProvider.GetSchemas())
            {
                _logger.LogInformation("Adding schema namespace {0} with support for types {1}", schema.SchemaNamespace, string.Join(";", schema.Types.Select(t => t.Name)));

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
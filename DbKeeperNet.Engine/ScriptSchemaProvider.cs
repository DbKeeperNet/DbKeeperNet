using System.Collections.Generic;

namespace DbKeeperNet.Engine
{
    public class ScriptSchemaProvider : IScriptSchemaProvider
    {
        private readonly IEnumerable<SchemaReference> _schemaReferences;

        public ScriptSchemaProvider(IEnumerable<SchemaReference> schemaReferences)
        {
            _schemaReferences = schemaReferences;
        }

        public IEnumerable<SchemaReference> GetSchemas()
        {
            return _schemaReferences;
        }
    }
}
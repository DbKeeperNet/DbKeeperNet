using System.Collections.Generic;

namespace DbKeeperNet.Engine
{
    public interface IScriptSchemaProvider
    {
        IEnumerable<SchemaReference> GetSchemas();
    }
}
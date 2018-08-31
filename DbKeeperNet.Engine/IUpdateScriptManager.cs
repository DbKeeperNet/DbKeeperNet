using System.Collections.Generic;
using System.IO;

namespace DbKeeperNet.Engine
{
    public interface IUpdateScriptManager
    {
        IEnumerable<Stream> Scripts { get; }
    }
}
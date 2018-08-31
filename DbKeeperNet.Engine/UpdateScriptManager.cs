using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DbKeeperNet.Engine
{
    public class UpdateScriptManager : IUpdateScriptManager
    {
        private readonly IEnumerable<IScriptProviderService> _scriptProviders;

        public UpdateScriptManager(IEnumerable<IScriptProviderService> scriptProviders)
        {
            _scriptProviders = scriptProviders;
        }

        public IEnumerable<Stream> Scripts
        {
            get { return _scriptProviders.Select(s => s.GetScriptStreamFromLocation()); }
        }
    }
}
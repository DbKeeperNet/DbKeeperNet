using System.IO;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Engine.ScriptProviders
{
    public class DiskFileUpdateScriptProvider : IScriptProviderService
    {
        private readonly ILogger<DiskFileUpdateScriptProvider> _logger;
        private readonly string _fileName;

        public DiskFileUpdateScriptProvider(ILogger<DiskFileUpdateScriptProvider> logger, string fileName)
        {
            _logger = logger;
            _fileName = fileName;
        }

        public Stream GetScriptStreamFromLocation()
        {
            _logger.LogInformation("Going to open update script {0}", _fileName);

            return File.OpenRead(_fileName);
        }
    }
}
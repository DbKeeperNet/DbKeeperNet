using System.IO;

namespace DbKeeperNet.Engine.ScriptProviders
{
    public class DiskFileUpdateScriptProvider : IScriptProviderService
    {
        private readonly string _fileName;

        public DiskFileUpdateScriptProvider(string fileName)
        {
            _fileName = fileName;
        }

        public Stream GetScriptStreamFromLocation()
        {
            return File.OpenRead(_fileName);
        }
    }
}
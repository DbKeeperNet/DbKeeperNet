using System;
using System.IO;
using System.Reflection;

namespace DbKeeperNet.Engine.ScriptProviders
{
    public class EmbeddedResourceUpdateScriptProvider : IScriptProviderService
    {
        private readonly string _resourceName;

        public EmbeddedResourceUpdateScriptProvider(string resourceName)
        {
            _resourceName = resourceName;
        }

        public Stream GetScriptStreamFromLocation()
        {
            var parameters = _resourceName.Split(',');

            if (parameters.Length != 2)
                throw new InvalidOperationException();

            var assemblyName = parameters[1];
            var resource = parameters[0];
            
            var assembly = Assembly.Load(new AssemblyName(assemblyName));
            var updates = assembly.GetManifestResourceStream(resource);

            if (updates == null)
            {
                throw new InvalidOperationException();
            }

            return updates;

        }
    }
}
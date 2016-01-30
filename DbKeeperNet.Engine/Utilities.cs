using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using DbKeeperNet.Engine.Resources;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Utility extension methods
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Get the manifest resource named <paramref name="resource"/>
        /// from assembly defined by type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">Type from which get assembly</param>
        /// <param name="resource">Resource name in the assembly</param>
        /// <returns></returns>
        public static Stream GetManifestResourceFromTypeAssembly(this Type type, string resource)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrEmpty(resource)) throw new ArgumentNullException(nameof(resource));

            var assembly = type.GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream(resource);

            if (stream == null)
            {
                throw new DbKeeperNetException(String.Format(CultureInfo.CurrentCulture, CommonMessages.ResourceNotFound, resource, assembly.FullName));
            }
            return stream;
        }
    }
}

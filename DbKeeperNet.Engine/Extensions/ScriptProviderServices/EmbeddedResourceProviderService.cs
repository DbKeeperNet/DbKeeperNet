using System;
using System.Reflection;
using System.IO;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.ScriptProviderServices
{
    /// <summary>
    /// Script provider allows to load scripts assembly embeded resource.
    /// </summary>
    /// <remarks>
    /// <para>Provider reference name is <c>asm</c>.</para>
    /// <para>
    /// Script name and assembly name are separated by comma in order: 
    /// <c>EmbededResource,Assembly</c>
    /// </para>
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this script provider
    /// in the App.Config.
    /// <code>
    /// <![CDATA[
    /// <updateScripts>
    ///   <add provider="asm" location="DbKeeperNet.ComplexDemo.DatabaseSetupPart1.xml,DbKeeperNet.ComplexDemo" />
    /// </updateScripts>
    /// ]]>
    /// </code>
    /// </example>
    public class EmbeddedResourceProviderService : IScriptProviderService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EmbeddedResourceProviderService(IUpdateContext context)
        {
            _context = context;
        }

        #region IScriptExecutionService Members

        /// <summary>
        /// Service registration name
        /// </summary>
        public string Name
        {
            get { return @"asm"; }
        }

        /// <summary>
        /// Method should open script based
        /// on the given location
        /// </summary>
        /// <param name="location">Location of this script. Location format depends
        /// on the used provider</param>
        public Stream GetScriptStreamFromLocation(string location)
        {
            if (string.IsNullOrEmpty(location)) throw new ArgumentNullException(@"location");

            string[] parameters = location.Split(new[] { ',' });

            if (parameters.Length != 2)
                throw new InvalidOperationException();

            string assemblyName = parameters[1];
            string resource = parameters[0];

            _context.Logger.TraceInformation(UpdaterMessages.StartingConfiguredUpdate, resource, assemblyName);

            Assembly assembly = Assembly.Load(assemblyName);
            Stream updates = assembly.GetManifestResourceStream(resource);

            if (updates == null)
            {
                _context.Logger.TraceError(UpdaterMessages.ManifestResourceNotFound, resource, assemblyName);
                throw new DbKeeperNetException(String.Format(CultureInfo.CurrentCulture, UpdaterMessages.ManifestResourceNotFound, resource, assemblyName));
            }

            return updates;
        }
        #endregion
        #region Private members
        private IUpdateContext _context;
        #endregion
    }
}

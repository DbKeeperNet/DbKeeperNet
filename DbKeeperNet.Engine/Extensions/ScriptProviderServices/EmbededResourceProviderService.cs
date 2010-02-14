using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.ScriptExecutionServices
{
    /// <summary>
    /// Script provider allows to load scripts assembly embeded resource.
    /// Script name and assembly name are separated by comma.
    /// Order is: EmbededResource,Assembly
    /// </summary>
    /// <remarks>
    /// Provider reference name is <c>asm</c>.
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
    public class EmbededResourceProviderService : IScriptProviderService
    {
        public EmbededResourceProviderService(IUpdateContext context)
        {
            _context = context;
        }

        #region IScriptExecutionService Members

        public string Name
        {
            get { return @"asm"; }
        }

        public Stream GetScriptStreamFromLocation(string location)
        {
            string[] parameters = location.Split(new char[] { ',' });

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

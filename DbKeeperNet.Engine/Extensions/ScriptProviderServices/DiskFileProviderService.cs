using System;
using System.IO;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.ScriptProviderServices
{
    /// <summary>
    /// Script provider allows to load scripts from disk.
    /// Location parameter can contain relative or absolute
    /// path to the executed script.
    /// </summary>
    /// <remarks>
    /// Provider reference name is <c>disk</c>.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this script provider
    /// in the App.Config.
    /// <code>
    /// <![CDATA[
    /// <updateScripts>
    ///   <add provider="disk" location="update.xml" />
    /// </updateScripts>
    /// ]]>
    /// </code>
    /// </example>
    public class DiskFileProviderService : IScriptProviderService
    {
        public DiskFileProviderService(IUpdateContext context)
        {
            _context = context;
        }

        #region IScriptExecutionService Members

        public string Name
        {
            get { return @"disk"; }
        }

        public Stream GetScriptStreamFromLocation(string location)
        {
            if (String.IsNullOrEmpty(location))
                throw new ArgumentNullException("location");

            string filePath = location;

            if (!File.Exists(filePath))
            {
                if (!Path.IsPathRooted(filePath))
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            }

            if (!File.Exists(filePath))
            {
                _context.Logger.TraceError(UpdaterMessages.FileUpdateNotFound, location);
                throw new DbKeeperNetException(String.Format(CultureInfo.CurrentCulture, UpdaterMessages.FileUpdateNotFound, filePath));
            }

            Stream stream = File.OpenRead(filePath);

            if (stream == null)
            {
                _context.Logger.TraceError(UpdaterMessages.FileUpdateNotFound, filePath);
            }

            return stream;
        }

        #endregion

        #region Private members
        private IUpdateContext _context;
        #endregion
    }
}

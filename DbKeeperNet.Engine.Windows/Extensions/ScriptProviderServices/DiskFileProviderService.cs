using System;
using System.Globalization;
using System.IO;
using DbKeeperNet.Engine.Windows.Resources;

namespace DbKeeperNet.Engine.Windows.Extensions.ScriptProviderServices
{
    /// <summary>
    /// Script provider allows to load scripts from disk.
    /// </summary>
    /// <remarks>
    /// <para>Provider reference name is <c>disk</c>.</para>
    /// <para>Location parameter can contain relative or absolute
    /// path to the executed script.</para>
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
                _context.Logger.TraceError(DiskFileProviderServiceMessages.FileUpdateNotFound, location);
                throw new DbKeeperNetException(String.Format(CultureInfo.CurrentCulture, DiskFileProviderServiceMessages.FileUpdateNotFound, filePath));
            }

            Stream stream = File.OpenRead(filePath);

            if (stream == null)
            {
                _context.Logger.TraceError(DiskFileProviderServiceMessages.FileUpdateNotFound, filePath);
            }

            return stream;
        }

#endregion

#region Private members
        private IUpdateContext _context;
#endregion
    }
}

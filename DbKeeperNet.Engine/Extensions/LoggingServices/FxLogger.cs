using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace DbKeeperNet.Engine.Extensions.LoggingServices
{
    /// <summary>
    /// Output is directed thru standard .NET logging service
    /// implemented by System.Diagnostics.Trace class and
    /// its static methods.
    /// 
    /// Reference name for configuration file is <code>fx</code>.
    /// </summary>
    public sealed class FxLogger : ILoggingService
    {
        #region ILoggingService Members

        public void TraceInformation(string format, params object[] p)
        {
            Trace.TraceInformation(format, p);
        }

        public void TraceError(string format, params object[] p)
        {
            Trace.TraceError(format, p);
        }

        public void TraceWarning(string format, params object[] p)
        {
            Trace.TraceWarning(format, p);
        }

        public string Name
        {
            get { return "fx"; }
        }
        #endregion
    }
}

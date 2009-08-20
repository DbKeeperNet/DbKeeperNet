using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace DbKeeperNet.Engine.Extensions.LoggingServices
{
    public sealed class FxTSLogger: ILoggingService
    {
        TraceSource ts = new TraceSource("DbKeeperNet");

        #region ILoggingService Members

        public string Name
        {
            get { return "fxts"; }
        }

        public void TraceInformation(string format, params object[] p)
        {
            ts.TraceEvent(TraceEventType.Information, 0, format, p);
        }

        public void TraceError(string format, params object[] p)
        {
            ts.TraceEvent(TraceEventType.Error, 0, format, p);
        }

        public void TraceWarning(string format, params object[] p)
        {
            ts.TraceEvent(TraceEventType.Warning, 0, format, p);
        }

        #endregion
    }
}

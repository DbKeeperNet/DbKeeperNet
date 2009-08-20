using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine.Extensions.LoggingServices
{
    public sealed class DummyLogger: ILoggingService
    {
        #region ILoggingService Members

        public string Name
        {
            get { return "dummy"; }
        }

        public void TraceInformation(string format, params object[] p)
        {
        }

        public void TraceError(string format, params object[] p)
        {
        }

        public void TraceWarning(string format, params object[] p)
        {
        }

        #endregion
    }
}

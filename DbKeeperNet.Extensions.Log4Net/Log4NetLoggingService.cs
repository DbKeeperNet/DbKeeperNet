using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine;
using log4net;

namespace DbKeeperNet.Extensions.Log4Net
{
    public sealed class Log4NetLoggingService: ILoggingService
    {
        private static ILog _logger = LogManager.GetLogger("DbKeeperNet");


        #region ILoggingService Members

        public string Name
        {
            get { return @"log4net"; }
        }

        public void TraceInformation(string format, params object[] p)
        {
            _logger.InfoFormat(format, p);
        }

        public void TraceError(string format, params object[] p)
        {
            _logger.ErrorFormat(format, p);
        }

        public void TraceWarning(string format, params object[] p)
        {
            _logger.WarnFormat(format, p);
        }

        #endregion
    }
}

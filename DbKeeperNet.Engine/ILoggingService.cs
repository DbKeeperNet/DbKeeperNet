using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine
{
    public interface ILoggingService
    {
        string Name { get; }
        void TraceInformation(string format, params object[] p);
        void TraceError(string format, params object[] p);
        void TraceWarning(string format, params object[] p);
    }
}

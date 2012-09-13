using System.Diagnostics;

namespace DbKeeperNet.Engine.Extensions.LoggingServices
{
    /// <summary>
    /// Output is directed thru standard .NET logging service
    /// implemented by System.Diagnostics.Trace class and
    /// its static methods.
    /// </summary>
    /// <example>
    /// Reference name for App.Config file is <c>fx</c>. App.Config extract below writes output
    /// to file <c>dbkeepernet.log</c>.
    /// <code>
    /// <![CDATA[
    /// <dbkeeper.net loggingService="fx">
    /// ...
    /// </dbkeeper.net>
    /// <system.diagnostics>
    ///   <trace autoflush="true">
    ///     <listeners>
    ///       <add name="file" initializeData="dbkeepernet.log" type="System.Diagnostics.TextWriterTraceListener" />
    ///     </listeners>
    ///   </trace>
    /// </system.diagnostics>
    /// ]]>
    /// </code>
    /// </example>
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

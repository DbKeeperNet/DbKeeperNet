using System.Diagnostics;

namespace DbKeeperNet.Engine.Windows.Extensions.LoggingServices
{
    /// <summary>
    /// Output is directed thru standard .NET logging service
    /// implemented by System.Diagnostics.TraceSource class.
    /// </summary>
    /// <remarks>
    /// Trace source name is <c>DbKeeperNet</c>.
    /// </remarks>
    /// <example>
    /// Reference name for App.Config file is <c>fxts</c>. App.Config extract below writes output
    /// to file <c>dbkeepernetts.log</c>.
    /// <code>
    /// <![CDATA[
    /// <dbkeeper.net loggingService="fxts">
    /// ...
    /// </dbkeeper.net>
    /// <system.diagnostics>
    ///   <sources>
    ///     <source name="DbKeeperNet" switchName="DbKeeperNet">
    ///       <listeners>
    ///         <add name="file" />
    ///       </listeners>
    ///     </source>
    ///   </sources>
    ///   <switches>
    ///     <add name="DbKeeperNet" value="Verbose"/>
    ///   </switches>
    ///   <sharedListeners>
    ///     <add name="file" initializeData="dbkeepernetts.log" type="System.Diagnostics.TextWriterTraceListener" />
    ///   </sharedListeners>
    ///   <trace autoflush="true">
    ///   </trace>
    /// </system.diagnostics>
    /// ]]>
    /// </code>
    /// </example>
    public sealed class FxTSLogger: ILoggingService
    {
        readonly TraceSource _traceSource = new TraceSource("DbKeeperNet");

        #region ILoggingService Members

        public string Name
        {
            get { return "fxts"; }
        }

        public void TraceInformation(string format, params object[] p)
        {
            _traceSource.TraceEvent(TraceEventType.Information, 0, format, p);
        }

        public void TraceError(string format, params object[] p)
        {
            _traceSource.TraceEvent(TraceEventType.Error, 0, format, p);
        }

        public void TraceWarning(string format, params object[] p)
        {
            _traceSource.TraceEvent(TraceEventType.Warning, 0, format, p);
        }

        #endregion
    }
}

namespace DbKeeperNet.Engine.Extensions.LoggingServices
{
    /// <summary>
    /// Dummy logging service, which completely ignores logged
    /// output.
    /// </summary>
    /// <example>
    /// Reference name for App.Config file is <c>dummy</c>.
    /// <code>
    /// <![CDATA[
    /// <dbkeeper.net loggingService="dummy">
    /// ...
    /// </dbkeeper.net>
    /// ]]>
    /// </code>
    /// </example>
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

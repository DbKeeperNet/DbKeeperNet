namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Contract definition for logging services.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Logging service name. Thru this value is
        /// service referenced in configuration file.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Write an information message thru this logging
        /// service.
        /// </summary>
        /// <param name="format">Standard .NET format string</param>
        /// <param name="p">Parameters referenced in format string.</param>
        void TraceInformation(string format, params object[] p);
        /// <summary>
        /// Write an error message thru this logging
        /// service.
        /// </summary>
        /// <param name="format">Standard .NET format string</param>
        /// <param name="p">Parameters referenced in format string.</param>
        void TraceError(string format, params object[] p);
        /// <summary>
        /// Write an warning message thru this logging
        /// service.
        /// </summary>
        /// <param name="format">Standard .NET format string</param>
        /// <param name="p">Parameters referenced in format string.</param>
        void TraceWarning(string format, params object[] p);
    }
}

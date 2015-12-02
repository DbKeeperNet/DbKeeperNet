using System;

namespace DbKeeperNet.Engine.Windows.Extensions.LoggingServices
{
    /// <summary>
    /// Registration entry point for built-in logging services
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>Registers support for following loggers:</listheader>
    /// <item><see cref="FxLogger">.NET Framework logging via System.Diagnostics.Trace</see></item>
    /// <item><see cref="FxTSLogger">.NET Framework logging via System.Diagnostics.TraceSource</see></item>
    /// </list>
    ///
    /// All providers implement interface <see cref="ILoggingService"/>
    /// </remarks>
    public sealed class LoggingServicesExtension: IExtension
    {
        #region IExtension Members

        public void Initialize(IUpdateContext context)
        {
            if (context == null) throw new ArgumentNullException(@"context");
                
            context.RegisterLoggingService(new FxLogger());
            context.RegisterLoggingService(new FxTSLogger());
        }

        #endregion
    }
}

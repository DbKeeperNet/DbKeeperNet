using System;

namespace DbKeeperNet.Engine.Extensions.LoggingServices
{
    /// <summary>
    /// Registration entry point for built-in logging services
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>Registers support for following loggers:</listheader>
    /// <item><see cref="DummyLogger">Dummy logger</see></item>
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

            context.RegisterLoggingService(new DummyLogger());
        }

        #endregion
    }
}

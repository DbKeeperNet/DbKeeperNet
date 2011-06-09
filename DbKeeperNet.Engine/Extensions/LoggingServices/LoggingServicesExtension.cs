namespace DbKeeperNet.Engine.Extensions.LoggingServices
{
    public sealed class LoggingServicesExtension: IExtension
    {
        #region IExtension Members

        public void Initialize(IUpdateContext context)
        {
            context.RegisterLoggingService(new FxLogger());
            context.RegisterLoggingService(new FxTSLogger());
            context.RegisterLoggingService(new DummyLogger());
        }

        #endregion
    }
}

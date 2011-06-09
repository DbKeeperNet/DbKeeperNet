using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Log4Net
{
    /// <summary>
    /// This extension adds support for Log4Net
    /// logging framework. All messages are written
    /// under DbKeeperNet logger.
    /// </summary>
    public class Log4NetExtension: IExtension
    {
        #region IExtension Members

        public void Initialize(IUpdateContext context)
        {
            context.RegisterLoggingService(new Log4NetLoggingService());
        }

        #endregion
    }
}

using DbKeeperNet.Engine.Extensions.ScriptProviderServices;

namespace DbKeeperNet.Engine.Windows.Extensions.ScriptProviderServices
{
    /// <summary>
    /// Registration entry point for built-in upgrade script providers
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>Registers support for following providers:</listheader>
    /// <item><see cref="DiskFileProviderService">Disk file provider</see></item>
    /// </list>
    /// 
    /// All providers implement interface <see cref="IScriptProviderService"/>
    /// </remarks>
    public class ScriptProviderServicesExtension: IExtension
    {
        #region IExtension Members

        public void Initialize(IUpdateContext context)
        {
            context.RegisterScriptProviderService(new DiskFileProviderService(context));
        }

        #endregion
    }
}

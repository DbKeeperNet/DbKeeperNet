namespace DbKeeperNet.Engine.Extensions.ScriptProviderServices
{
    /// <summary>
    /// Registration entry point for built-in upgrade script providers
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>Registers support for following providers:</listheader>
    /// <item><see cref="EmbededResourceProviderService">Embeded resource provider</see></item>
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
            context.RegisterScriptProviderService(new EmbededResourceProviderService(context));
            context.RegisterScriptProviderService(new DiskFileProviderService(context));
        }

        #endregion
    }
}

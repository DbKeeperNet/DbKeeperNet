namespace DbKeeperNet.Engine.Extensions.ScriptProviderServices
{
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

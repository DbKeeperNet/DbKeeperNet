namespace DbKeeperNet.Engine
{
    public class UpdateContext : ScriptContext
    {
        public UpdateContext(UpdateType update, ScriptContext scriptContext) : base(scriptContext)
        {
            UpdateVersion = update.Version;
            FriendlyName = update.FriendlyName;
        }

        public UpdateContext(UpdateContext context) : base(context)
        {
            UpdateVersion = context.UpdateVersion;
            FriendlyName = context.FriendlyName;
        }

        public string UpdateVersion { get; }
        public string FriendlyName { get; }

        public override string ToString()
        {
            return $"Updates[{AssemblyName},version={UpdateVersion}]";
        }
    }
}
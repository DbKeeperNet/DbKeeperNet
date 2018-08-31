namespace DbKeeperNet.Engine
{
    public class ScriptContext
    {
        public ScriptContext(Updates update)
        {
            AssemblyName = update.AssemblyName;
        }

        public ScriptContext(ScriptContext context)
        {
            AssemblyName = context.AssemblyName;
        }

        public string AssemblyName { get; set; }

        public override string ToString()
        {
            return $"Updates[{AssemblyName}]";
        }
    }
}
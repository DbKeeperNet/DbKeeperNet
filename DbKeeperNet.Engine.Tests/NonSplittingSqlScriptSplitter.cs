using System.Collections.Generic;

namespace DbKeeperNet.Engine.Tests
{
    public class NonSplittingSqlScriptSplitter: ISqlScriptSplitter
    {
        public IEnumerable<string> SplitScript(string script)
        {
            yield return script;
        }
    }
}
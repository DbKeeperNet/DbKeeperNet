using System.Collections.Generic;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Strategy implementing splitting of multiple commands
    /// to single commands.
    /// </summary>
    /// <remarks>
    /// Some database connectors do not support multiple commands
    /// within single command text.
    /// </remarks>
    public interface ISqlScriptSplitter
    {
        /// <summary>
        /// Split the given SQL script possibly containing multiple commands
        /// to single commands which can be executed by database service.
        /// </summary>
        /// <param name="script">
        /// SQL script to be executed:
        /// <list type="bullet">
        /// <item>Single script without any delimiter</item>
        /// <item>Multiple commands delimited by a delimiter (e.g. <c><![CDATA[<GO>]]></c>). Last command must be terminated as well.</item>
        /// </list>
        /// </param>
        /// <returns>Collection of string representing single commands</returns>
        IEnumerable<string> SplitScript(string script);
    }
}
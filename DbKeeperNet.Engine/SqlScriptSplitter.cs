using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Default implementation of <see cref="ISqlScriptSplitter"/>
    /// </summary>
    /// <remarks>
    /// <para>Commands are splitted using <c><![CDATA[<GO>]]></c> statement on separated line.</para>
    /// <para>Each command is terminated by end-of-line.</para>
    /// <example>
    /// Command without delimiter:
    /// <code>
    /// SELECT * FROM Test
    /// </code>
    /// 
    /// Command with delimiter:
    /// <code>
    /// <![CDATA[
    /// SELECT * FROM Test
    /// <GO>
    /// EXEC sp_Update_Data
    /// <GO>
    /// ]]>
    /// </code>
    /// </example>
    /// </remarks>
    public class SqlScriptSplitter:ISqlScriptSplitter
    {
        private const string _commandSeparator = @"<GO>";

        /// <summary>
        /// Split the given SQL script possibly containing multiple commands
        /// to single commands which can be executed by database service.
        /// </summary>
        /// <param name="script">
        /// SQL script to be executed:
        /// <list type="bullet">
        /// <item>Single script without any delimiter</item>
        /// <item>Multiple commands delimited by a delimiter (e.g. <c><![CDATA[<GO>]]></c>. Last command must be terminated as well.</item>
        /// </list>
        /// </param>
        /// <returns>Collection of string representing single commands</returns>
        public IEnumerable<string> SplitScript(string script)
        {
            using(var reader = new StringReader(script))
            {
                var command = new StringBuilder();
                var terminationFound = false;

                while(true)
                {
                    var line = reader.ReadLine();

                    if (line == null) break;

                    if (line.Trim().Equals(_commandSeparator))
                    {
                        terminationFound = true;

                        var commandToRun = command.ToString();
                        command = new StringBuilder();

                        yield return commandToRun;
                    }
                    else
                    {
                        command.AppendLine(line);
                    }
                }

                if (!terminationFound) yield return command.ToString();
            }
        }
    }
}
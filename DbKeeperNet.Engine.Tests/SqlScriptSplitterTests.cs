using System.Collections.Generic;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests
{
    /// <summary>
    /// Test class for <see cref="SqlScriptSplitter"/>
    /// </summary>
    [TestFixture]
    public class SqlScriptSplitterTests
    {
        const string ScriptWithoutDelimiter = "SELECT * FROM a\r\nA\r\nWHERE 1 = 2\r\n\r\n\r\n";
        const string ScriptWithDelimiterOnCommandLine = "SELECT * FROM <GO>\r\nA\r\nWHERE 1 = 2\r\n\r\n\r\n";
        const string FirstScript = "SELECT * FROM\r\nA\r\n\r\n\r\n";
        const string SecondScript = "INSERT INTO \r\n";
        const string ScriptWithTwoDelimiters = FirstScript + "<GO>\r\n" + SecondScript + "<GO>\r\n\r\n";

        [Test]
        public void SplitScriptShouldReturnSameSingleScriptIfNoDelimiterIsFound()
        {
            var splitter = new SqlScriptSplitter();

            var scripts = new List<string>(splitter.SplitScript(ScriptWithoutDelimiter));

            Assert.That(scripts.Count, Is.EqualTo(1));
            Assert.That(scripts[0], Is.EqualTo(ScriptWithoutDelimiter));
        }

        [Test]
        public void SplitScriptShouldReturnTwoScriptsIfTwoDelimitersAreFound()
        {
            var splitter = new SqlScriptSplitter();

            var scripts = new List<string>(splitter.SplitScript(ScriptWithTwoDelimiters));

            Assert.That(scripts.Count, Is.EqualTo(2));
            Assert.That(scripts[0], Is.EqualTo(FirstScript));
            Assert.That(scripts[1], Is.EqualTo(SecondScript));
        }

        [Test]
        public void SplitScriptShouldReturnSameSingleScriptIfDelimiterIsNotOnSeparatedLine()
        {
            var splitter = new SqlScriptSplitter();

            var scripts = new List<string>(splitter.SplitScript(ScriptWithDelimiterOnCommandLine));

            Assert.That(scripts.Count, Is.EqualTo(1));
            Assert.That(scripts[0], Is.EqualTo(ScriptWithDelimiterOnCommandLine));           
        }
    }
}
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests
{
    /// <summary>
    /// Test class for <see cref="MsSqlScriptSplitter"/>
    /// </summary>
    [TestFixture]
    public class MsSqlScriptSplitterTests
    {
        const string ScriptWithoutDelimiter = "SELECT * FROM a\r\nA\r\nWHERE 1 = 2\r\n\r\n\r\n";
        const string ScriptWithDelimiterOnCommandLine = "SELECT * FROM GO\r\nA\r\nWHERE 1 = 2\r\n\r\n\r\n";
        const string FirstScript = "SELECT * FROM\r\nA\r\n\r\n\r\n";
        const string SecondScript = "INSERT INTO \r\n";
        const string ScriptWithTwoDelimiters = FirstScript + "GO\r\n" + SecondScript + "GO\r\n\r\n";

        [Test]
        public void SplitScriptShouldReturnSameSingleScriptIfNoDelimiterIsFound()
        {
            var splitter = new MsSqlScriptSplitter();

            var scripts = new List<string>(splitter.SplitScript(ScriptWithoutDelimiter));

            Assert.That(scripts.Count, Is.EqualTo(1));
            
            AssertThatValueMatches(scripts[0], ScriptWithoutDelimiter);
        }

        [Test]
        public void SplitScriptShouldReturnTwoScriptsIfTwoDelimitersAreFound()
        {
            var splitter = new MsSqlScriptSplitter();

            var scripts = new List<string>(splitter.SplitScript(ScriptWithTwoDelimiters));

            Assert.That(scripts.Count, Is.EqualTo(2));
            AssertThatValueMatches(scripts[0], FirstScript);
            AssertThatValueMatches(scripts[1], SecondScript);
        }

        [Test]
        public void SplitScriptShouldReturnSameSingleScriptIfDelimiterIsNotOnSeparatedLine()
        {
            var splitter = new MsSqlScriptSplitter();

            var scripts = new List<string>(splitter.SplitScript(ScriptWithDelimiterOnCommandLine));

            Assert.That(scripts.Count, Is.EqualTo(1));
            AssertThatValueMatches(scripts[0], ScriptWithDelimiterOnCommandLine);
        }

        private static void AssertThatValueMatches(string actual, string expectedValue)
        {
            Assert.That(actual, Is.EqualTo(expectedValue.Replace("\r\n", Environment.NewLine)));
        }

    }
}
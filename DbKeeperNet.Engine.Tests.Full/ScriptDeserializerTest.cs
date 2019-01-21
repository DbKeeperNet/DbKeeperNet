using System.IO;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Full
{
    [TestFixture]
    public class ScriptDeserializerTest
    {
        [Test]
        public void ShouldThrowArgumentNullExceptionWhenStreamNotProvider()
        {
            var instance = CreateDeserializer();

            Assert.That(() => instance.GetDeserializedScript(null), Throws.ArgumentNullException);
        }

        [Test]
        public void ShouldThrowInvalidOperationExceptionWhenScriptCannotBeDeserialized()
        {
            var instance = CreateDeserializer();

            Assert.That(() => instance.GetDeserializedScript(new MemoryStream(Encoding.ASCII.GetBytes("Invalid xml"))), Throws.InvalidOperationException);
        }

        [Test]
        public void ShouldReturnDeserializedScriptForValidXml()
        {
            var instance = CreateDeserializer();

            var script = instance.GetDeserializedScript(new MemoryStream(Encoding.ASCII.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<upd:Updates xmlns:upd=""http://code.google.com/p/dbkeepernet/Updates-1.0.xsd""
                	xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
	              xsi:schemaLocation=""http://code.google.com/p/dbkeepernet/Updates-1.0.xsd Updates-1.0.xsd""
                AssemblyName=""DbUpdater.Engine"" />
")));
            Assert.That(script, Is.Not.Null);
        }

        private ScriptDeserializer CreateDeserializer()
        {
            return new ScriptDeserializer(NullLogger<ScriptDeserializer>.Instance, new Mock<IScriptSchemaProvider>().Object);
        }
    }
}
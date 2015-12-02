using System;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests
{
    [TestFixture]
    public class UtilitiesTest
    {
        [Test]
        public void GetManifestResourceFromTypeAssemblyShouldThrowNullArgumentExceptionWhenTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Utilities.GetManifestResourceFromTypeAssembly(null, "resource"));
        }

        [Test]
        public void GetManifestResourceFromTypeAssemblyShouldThrowNullArgumentExceptionWhenResourceIsEmptyString()
        {
            Assert.Throws<ArgumentNullException>(() => Utilities.GetManifestResourceFromTypeAssembly(GetType(), string.Empty));
        }

        [Test]
        public void GetManifestResourceFromTypeAssemblyShouldThrowNullArgumentExceptionWhenResourceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Utilities.GetManifestResourceFromTypeAssembly(GetType(), null));
        }

        [Test]
        public void GetManifestResourceFromTypeAssemblyShouldThrowDbKeeperNetExceptionWhenResourceIsNotFound()
        {
            Assert.Throws<DbKeeperNetException>(() => Utilities.GetManifestResourceFromTypeAssembly(GetType(), "UnknownResource"));
        }

        [Test]
        public void GetManifestResourceFromTypeAssemblyShouldReturnResourceStreamForValidResourceName()
        {
            using (var resource = Utilities.GetManifestResourceFromTypeAssembly(GetType(), "DbKeeperNet.Engine.Tests.CustomUpdateStep.xml"))
            {
                Assert.IsNotNull(resource);
            }
        }
    }
}
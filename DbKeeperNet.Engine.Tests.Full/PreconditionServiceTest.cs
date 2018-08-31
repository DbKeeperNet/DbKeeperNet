using System;
using System.Linq;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Full
{
    [TestFixture]
    public class PreconditionServiceTest
    {
        // TODO: fix
        //[Test]
        //public void ShouldReturnTrueForEmptyPreconditions()
        //{
        //    var service = CreateService();

        //    var result = service.IsMet(new UpdateStepContextPrecondition());

        //    Assert.That(result, Is.True);
        //}

        [Test]
        public void ShouldThrowArgumentNullExceptionForNullPreconditions()
        {
            var service = CreateService();

            Assert.That(() => service.IsMet(null), Throws.ArgumentNullException);

        }

        private PreconditionService CreateService()
        {
            return new PreconditionService(null);
        }
    }
}
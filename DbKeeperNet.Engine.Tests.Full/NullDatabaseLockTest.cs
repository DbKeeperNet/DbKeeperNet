using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Full
{
    [TestFixture]
    public class NullDatabaseLockTest
    {
        [Test]
        public void IsSupportedShouldReturnFalse()
        {
            var databaseLock = CreateNullDatabaseLock();

            Assert.That(databaseLock.IsSupported, Is.False);
        }

        private static NullDatabaseLock CreateNullDatabaseLock()
        {
            var databaseLock = new NullDatabaseLock();
            return databaseLock;
        }

        [Test]
        public void ReleaseShouldDoNothing()
        {
            var databaseLock = CreateNullDatabaseLock();

            databaseLock.Release(0);
        }

        [Test]
        public void AcquireShouldReturnTrue()
        {
            var databaseLock = CreateNullDatabaseLock();

            Assert.That(databaseLock.Acquire(0, null, 0), Is.True);
        }
    }
}
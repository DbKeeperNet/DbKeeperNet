using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Full
{
    /// <summary>
    /// Test class for <see cref="DatabaseLockService"/>
    /// </summary>
    [TestFixture]
    public class DatabaseLockServiceTest
    {
        private Mock<IDatabaseLock> _databaseLock;
        private const int LockId = 1;

        [SetUp]
        public void Setup()
        {
            _databaseLock = new Mock<IDatabaseLock>(MockBehavior.Strict);
        }

        [Test]
        public void IfDatabaseLockIsNotSupportedLockShouldDoNothing()
        {
            _databaseLock.Setup(l => l.IsSupported).Returns(false);

            var service = CreateDatabaseLockService();
            
            var locker = service.AcquireLock(LockId);
            locker.Dispose();
        }

        [Test]
        public void IfDatabaseLockIsSupportedAndAcquiredDisposeShouldRelease()
        {
            _databaseLock.Setup(l => l.IsSupported).Returns(true);
            _databaseLock.Setup(l => l.Acquire(LockId, It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            _databaseLock.Setup(l => l.Release(LockId));

            var service = CreateDatabaseLockService();
            
            var locker = service.AcquireLock(LockId);

            _databaseLock.Verify(l => l.Acquire(LockId, It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            _databaseLock.Verify(l => l.Release(LockId), Times.Never);

            locker.Dispose();

            _databaseLock.Verify(l => l.Release(LockId), Times.Once);
        }

        [Test]
        public void AcquireShouldTryUntilItSucceeds()
        {
            var sequence = new MockSequence();
            _databaseLock.Setup(l => l.IsSupported).Returns(true);
            _databaseLock.InSequence(sequence).Setup(l => l.Acquire(LockId, It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            _databaseLock.InSequence(sequence).Setup(l => l.Acquire(LockId, It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            _databaseLock.Setup(l => l.Release(LockId));

            var service = CreateDatabaseLockService();
            
            var locker = service.AcquireLock(LockId);

            _databaseLock.Verify(l => l.Acquire(LockId, It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(2));
            _databaseLock.Verify(l => l.Release(LockId), Times.Never);

            locker.Dispose();

            _databaseLock.Verify(l => l.Release(LockId), Times.Once);
        }

        private DatabaseLockService CreateDatabaseLockService()
        {
            var service = new DatabaseLockService(NullLogger<DatabaseLockService>.Instance, _databaseLock.Object);

            return service;
        }
    }
}
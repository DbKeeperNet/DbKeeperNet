using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests
{
    /// <summary>
    /// Base test class to excercise <see cref="IDatabaseLock"/> implementations
    /// </summary>
    public abstract class DatabaseLockTests : TestBase
    {
        protected const int TestLockId = 4262;
        
        [SetUp]
        public override void Setup()
        {
            base.Setup();

            var upgrader = DefaultScope.ServiceProvider.GetService<IDatabaseUpdater>();
            upgrader.ExecuteUpgrade();

            Reset();
        }

        [TearDown]
        public override void Shutdown()
        {
            Reset();

            base.Shutdown();
        }

        protected abstract void Reset();
        
        [Test]
        public void SimpleAcquireShouldWorkProperly()
        {
            var databaseLock = GetService<IDatabaseLock>();

            var acquried = databaseLock.Acquire(TestLockId, "Unit test", 5);
            Assert.That(acquried, Is.True);
        }

        [Test]
        public void SimpleReleaseShouldWorkProperly()
        {
            var databaseLock = GetService<IDatabaseLock>();

            Assert.DoesNotThrow(() => databaseLock.Release(TestLockId));
        }

        [Test]
        public void NestedLockShouldNotAcquireAlreadyAcquiredLock()
        {
            var databaseLock = GetService<IDatabaseLock>();

            var acquried = databaseLock.Acquire(TestLockId, "Unit test", 5);
            Assert.That(acquried, Is.True);

            var nestedAcquire = databaseLock.Acquire(TestLockId, "Unit test", 5);
            Assert.That(nestedAcquire, Is.False);
        }

        [Test]
        public void SubsequentAcquireAndReleaseShouldWorkProperly()
        {
            var databaseLock = GetService<IDatabaseLock>();

            var acquried = databaseLock.Acquire(TestLockId, "Unit test", 5);
            Assert.That(acquried, Is.True);
            databaseLock.Release(TestLockId);

            var subsequentAcquire = databaseLock.Acquire(TestLockId, "Unit test", 5);
            Assert.That(subsequentAcquire, Is.True);
        }
    }
}
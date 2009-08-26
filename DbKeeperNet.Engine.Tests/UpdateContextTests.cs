using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace DbKeeperNet.Engine.Tests
{
    [TestFixture]
    public class UpdateContextTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterPreconditionNull()
        {
            IUpdateContext context = new UpdateContext();
            context.RegisterPrecondition(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterPreconditionNullName()
        {
            MockRepository repository = new MockRepository();
            IPrecondition mockPrec = repository.DynamicMock<IPrecondition>();

            using (repository.Record())
            {
                SetupResult.For(mockPrec.Name).Return(null);
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();
                context.RegisterPrecondition(mockPrec);
            }
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterPreconditionEmptyName()
        {
            MockRepository repository = new MockRepository();
            IPrecondition mockPrec = repository.DynamicMock<IPrecondition>();

            using (repository.Record())
            {
                SetupResult.For(mockPrec.Name).Return("");
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();
                context.RegisterPrecondition(mockPrec);
            }
        }
        [Test]
        public void RegisterPrecondition()
        {
            MockRepository repository = new MockRepository();
            IPrecondition mockPrec = repository.DynamicMock<IPrecondition>();

            using (repository.Record())
            {
                SetupResult.For(mockPrec.Name).Return("testingPrecondition");
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();
                context.RegisterPrecondition(mockPrec);
            }
        }
        [Test]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CheckNonExistingPrecondition()
        {
            MockRepository repository = new MockRepository();
            IPrecondition mockPrec = repository.DynamicMock<IPrecondition>();

            using (repository.Record())
            {
                SetupResult.For(mockPrec.Name).Return("testingPrecondition");
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();
                context.RegisterPrecondition(mockPrec);
                context.CheckPrecondition("xxx", null);
            }
        }
        [Test]
        public void CheckPrecondition()
        {
            MockRepository repository = new MockRepository();
            IPrecondition mockPrec = repository.DynamicMock<IPrecondition>();
            IUpdateContext context = new UpdateContext();

            using (repository.Record())
            {
                SetupResult.For(mockPrec.Name).Return("testingPrecondition");

                Expect.Call(mockPrec.CheckPrecondition(context, null)).Return(true);
                Expect.Call(mockPrec.CheckPrecondition(context, null)).Return(false);
            }

            using (repository.Playback())
            {
                context.RegisterPrecondition(mockPrec);
                Assert.That(context.CheckPrecondition("testingPrecondition", null), Is.EqualTo(true));
                Assert.That(context.CheckPrecondition("testingPrecondition", null), Is.EqualTo(false));
            }

            repository.VerifyAll();
        }

        [Test]
        public void RegisterDatabaseService()
        {
            MockRepository repository = new MockRepository();
            IDatabaseService mockService = repository.DynamicMock<IDatabaseService>();

            using (repository.Record())
            {
                SetupResult.For(mockService.Name).Return("testingService");
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();
                context.RegisterDatabaseService(mockService);
            }
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterDatabaseServiceNullName()
        {
            MockRepository repository = new MockRepository();
            IDatabaseService mockService = repository.DynamicMock<IDatabaseService>();

            using (repository.Record())
            {
                SetupResult.For(mockService.Name).Return(null);
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();
                context.RegisterDatabaseService(mockService);
            }
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterDatabaseServiceEmptyName()
        {
            MockRepository repository = new MockRepository();
            IDatabaseService mockService = repository.DynamicMock<IDatabaseService>();

            using (repository.Record())
            {
                SetupResult.For(mockService.Name).Return("");
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();
                context.RegisterDatabaseService(mockService);
            }
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterDatabaseServiceNull()
        {
            IUpdateContext context = new UpdateContext();
            context.RegisterDatabaseService(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterLoggingServiceNullName()
        {
            MockRepository repository = new MockRepository();
            ILoggingService mockService = repository.DynamicMock<ILoggingService>();

            using (repository.Record())
            {
                SetupResult.For(mockService.Name).Return(null);
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();
                context.RegisterLoggingService(mockService);
            }
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterLoggingServiceEmptyName()
        {
            MockRepository repository = new MockRepository();
            ILoggingService mockService = repository.DynamicMock<ILoggingService>();

            using (repository.Record())
            {
                SetupResult.For(mockService.Name).Return("");
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();
                context.RegisterLoggingService(mockService);
            }
        }
        [Test]
        public void RegisterLoggingService()
        {
            MockRepository repository = new MockRepository();
            ILoggingService mockService = repository.DynamicMock<ILoggingService>();

            using (repository.Record())
            {
                SetupResult.For(mockService.Name).Return("testingLogger");
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext();
                context.RegisterLoggingService(mockService);
            }
        }
    }
}
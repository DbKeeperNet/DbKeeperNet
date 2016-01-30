using System;
using System.Collections.Generic;
using DbKeeperNet.Engine.Tests.Extensions.Preconditions;
using NUnit.Framework;
using Rhino.Mocks;

namespace DbKeeperNet.Engine.Tests
{
    [TestFixture]
    public class UpdateContextTests
    {
        [Test]
        public void RegisterPreconditionNull()
        {
            IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
            Assert.Throws<ArgumentNullException>(() => context.RegisterPrecondition(null));
        }

        [Test]
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
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
                Assert.Throws<InvalidOperationException>(() => context.RegisterPrecondition(mockPrec));
            }
        }

        [Test]
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
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
                Assert.Throws<InvalidOperationException>(() => context.RegisterPrecondition(mockPrec));
            }
        }

        [Test]
        public void SchemaRegistration()
        {
            var context = new UpdateContext(new TestDbKeeperNetConfiguration());
            context.RegisterSchema("someUri", "schema");

            string uri = null;
            string schema = null;

            context.IterateAllSchemas((registeredUri, registereSchema) =>
                                      {
                                          uri = registeredUri;
                                          schema = registereSchema;
            });

            Assert.That(uri, Is.EqualTo("someUri"));
            Assert.That(schema, Is.EqualTo("schema"));
        }

        [Test]
        public void RegisterSchemaNullUri()
        {
            var context = new UpdateContext(new TestDbKeeperNetConfiguration());
            Assert.Throws<ArgumentNullException>(() => context.RegisterSchema(null, "schema"));
        }

        [Test]
        public void RegisterSchemaNullSchema()
        {
            var context = new UpdateContext(new TestDbKeeperNetConfiguration());
            Assert.Throws<ArgumentNullException>(() => context.RegisterSchema("someUri", null));
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
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
                context.RegisterPrecondition(mockPrec);
            }
        }

        [Test]
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
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
                context.RegisterPrecondition(mockPrec);
                Assert.Throws<KeyNotFoundException>(() => context.CheckPrecondition("xxx", null));
            }
        }

        [Test]
        public void CheckPrecondition()
        {
            MockRepository repository = new MockRepository();
            IPrecondition mockPrec = repository.DynamicMock<IPrecondition>();
            IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());

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
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
                context.RegisterDatabaseService(mockService);
            }
        }

        [Test]
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
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
                Assert.Throws<InvalidOperationException>(() => context.RegisterDatabaseService(mockService));
            }
        }

        [Test]
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
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
                Assert.Throws<InvalidOperationException>(() => context.RegisterDatabaseService(mockService));
            }
        }

        [Test]
        public void RegisterDatabaseServiceNull()
        {
            IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
            Assert.Throws<ArgumentNullException>(() => context.RegisterDatabaseService(null));
        }

        [Test]
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
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
                Assert.Throws<InvalidOperationException>(() => context.RegisterLoggingService(mockService));
            }
        }

        [Test]
        public void RegisterUpdateStepHandlerServiceNull()
        {
            var context = new UpdateContext(new TestDbKeeperNetConfiguration());
            Assert.Throws<ArgumentNullException>(() =>context.RegisterUpdateStepHandler(null));
        }

        [Test]
        public void RegisterUpdateStepHandlerService()
        {
            var repository = new MockRepository();
            var service = repository.DynamicMock<IUpdateStepHandlerService>();

            using (repository.Record())
            {
                SetupResult.For(service.HandledType).Return(typeof(UpdateStepBaseType));
            }

            using (repository.Playback())
            {
                var context = new UpdateContext(new TestDbKeeperNetConfiguration());
                context.RegisterUpdateStepHandler(service);
            }
        }

        [Test]
        public void GetUpdateStepHandler()
        {
            var repository = new MockRepository();
            var service = repository.DynamicMock<IUpdateStepHandlerService>();

            using (repository.Record())
            {
                SetupResult.For(service.HandledType).Return(typeof(UpdateDbStepType));
            }

            using (repository.Playback())
            {
                var context = new UpdateContext(new TestDbKeeperNetConfiguration());
                context.RegisterUpdateStepHandler(service);

                Assert.That(context.GetUpdateStepHandlerFor(new UpdateDbStepType()), Is.EqualTo(service));
            }
        }

        [Test]
        public void GetUpdateStepHandlerNullStep()
        {
            var context = new UpdateContext(new TestDbKeeperNetConfiguration());

            Assert.Throws<ArgumentNullException>(() => context.GetUpdateStepHandlerFor(null));
        }

        [Test]
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
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
                Assert.Throws<InvalidOperationException>(() => context.RegisterLoggingService(mockService));
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
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
                context.RegisterLoggingService(mockService);
            }
        }

        [Test]
        public void InitializeDatabaseServiceUsingDatabaseServiceShouldThrowExceptionForNullArgument()
        {
            using(IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration()))
            {
                Assert.Throws<ArgumentNullException>(() => context.InitializeDatabaseService((IDatabaseService)null, false));
            }
        }

        [Test]
        public void InitializeDatabaseServiceUsingDatabaseServiceShouldWork()
        {
            MockRepository mockRepository = new MockRepository();
            IDatabaseService databaseService = mockRepository.DynamicMock<IDatabaseService>();

            using (IUpdateContext context = CreateAContext())
            {
                context.InitializeDatabaseService(databaseService, false);
            }
        }

        [Test]
        public void RegisterScriptExecutionService()
        {
            MockRepository repository = new MockRepository();
            IScriptProviderService mockService = repository.DynamicMock<IScriptProviderService>();

            using (repository.Record())
            {
                SetupResult.For(mockService.Name).Return("testingScriptExecition");
            }

            using (repository.Playback())
            {
                IUpdateContext context = new UpdateContext(new TestDbKeeperNetConfiguration());
                context.RegisterScriptProviderService(mockService);
            }

            repository.VerifyAll();
        }

        [Test]
        public void InitializeServiceWithADatabaseServiceAndDisposeServiceTrueShouldDisposeServiceDuringContextDispose()
        {
            var repository = new MockRepository();
            var databaseService = repository.DynamicMock<IDatabaseService>();

            using(repository.Record())
            {
                Expect.Call(databaseService.Dispose);
            }

            using (repository.Playback())
            {
                var context = CreateAContext();

                context.InitializeDatabaseService(databaseService, true);

                context.Dispose();
            }
        }

        [Test]
        public void InitializeServiceWithADatabaseServiceAndDisposeServiceFalseShouldNotDisposeServiceDuringContextDispose()
        {
            var repository = new MockRepository();
            var databaseService = repository.DynamicMock<IDatabaseService>();

            using (repository.Record())
            {
                DoNotExpect.Call(databaseService.Dispose);
            }

            using (repository.Playback())
            {
                var context = CreateAContext();

                context.InitializeDatabaseService(databaseService, false);

                context.Dispose();
            }
        }

        private static UpdateContext CreateAContext()
        {
            var context = new UpdateContext(new TestDbKeeperNetConfiguration());

            context.LoadExtensions();
            context.InitializeLoggingService(@"dummy");
            return context;
        }
    }
}

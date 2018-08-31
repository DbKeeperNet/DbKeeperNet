using DbKeeperNet.Engine.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Checkers
{
    public abstract class UpdateStepExecutedCheckerBase : TestBase
    {
        private IDatabaseServiceTransactionProvider _transactionService;
        private const string ExistingAssembly = "ExistingAssembly";
        private const string ExistingVersion = "ExistingVersion";
        private const int ExistingStep = 13;

        private const string NonExistingAssembly = "NonExistingAssembly";
        private const string NonExistingVersion = "NonExistingVersion";
        private const int NonExistingStep = 12;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            Cleanup();

            // Setup DB
            var upgrader = DefaultScope.ServiceProvider.GetService<IDatabaseUpdater>();
            upgrader.ExecuteUpgrade();

            _transactionService = DefaultScope.ServiceProvider.GetService<IDatabaseServiceTransactionProvider>();

            _transactionService.BeginTransaction();
            var updateStepMarker = DefaultScope.ServiceProvider.GetService<IUpdateStepExecutedMarker>();
            updateStepMarker.MarkAsExecuted(ExistingAssembly, ExistingVersion, ExistingStep);
            _transactionService.CommitTransaction();
        }

        [TearDown]
        public override void Shutdown()
        {
            Cleanup();

            base.Shutdown();
        }

        protected abstract void Cleanup();


        [TestCase(ExistingAssembly, ExistingVersion, NonExistingStep)]
        [TestCase(ExistingAssembly, NonExistingVersion, ExistingStep)]
        [TestCase(NonExistingAssembly, ExistingVersion, ExistingStep)]
        [TestCase(ExistingAssembly, NonExistingVersion, NonExistingStep)]
        [TestCase(NonExistingAssembly, NonExistingVersion, ExistingStep)]
        [TestCase(NonExistingAssembly, NonExistingVersion, NonExistingStep)]
        [TestCase(NonExistingAssembly, ExistingVersion, NonExistingStep)]
        public void ExecutedShouldReturnFalseIfStepWasNotExecutedYet(string assembly, string version, int step)
        {
            _transactionService.BeginTransaction();
            var checker = DefaultScope.ServiceProvider.GetService<IUpdateStepExecutedChecker>();
            _transactionService.CommitTransaction();

            Assert.That(checker.IsExecuted(assembly, version, step), Is.False);
        }

        [Test]
        public void ExecutedShouldReturnTrueIfStepWasExecuted()
        {
            var checker = DefaultScope.ServiceProvider.GetService<IUpdateStepExecutedChecker>();

            Assert.That(checker.IsExecuted(ExistingAssembly, ExistingVersion, ExistingStep), Is.True);
        }
    }
}
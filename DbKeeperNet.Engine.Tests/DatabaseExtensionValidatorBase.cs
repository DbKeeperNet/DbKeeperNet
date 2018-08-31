using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests
{
    public abstract class DatabaseExtensionValidatorBase : TestBase
    {

        [Test]
        public void IDatabaseServiceInstallerShouldBeRegister()
        {
            var instance = GetService<IDatabaseServiceInstaller>();

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void IUpdateStepExecutedMarkerShouldBeRegister()
        {
            var instance = GetService<IUpdateStepExecutedMarker>();

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void IUpdateStepExecutedCheckerShouldBeRegister()
        {
            var instance = GetService<IUpdateStepExecutedChecker>();

            Assert.That(instance, Is.Not.Null);
        }


        [Test]
        public void IDatabaseServiceCommandHandlerShouldBeRegister()
        {
            var instance = GetService<IDatabaseServiceCommandHandler>();

            Assert.That(instance, Is.Not.Null);
        }


        [Test]
        public void IDatabaseServiceForeignKeyCheckerShouldBeRegister()
        {
            var instance = GetService<IDatabaseServiceForeignKeyChecker>();

            Assert.That(instance, Is.Not.Null);
        }


        [Test]
        public void IDatabaseServicePrimaryKeyCheckerShouldBeRegister()
        {
            var instance = GetService<IDatabaseServicePrimaryKeyChecker>();

            Assert.That(instance, Is.Not.Null);
        }


        [Test]
        public void IDatabaseServiceIndexCheckerShouldBeRegister()
        {
            var instance = GetService<IDatabaseServiceIndexChecker>();

            Assert.That(instance, Is.Not.Null);
        }



        [Test]
        public void IDatabaseServiceTableCheckerShouldBeRegister()
        {
            var instance = GetService<IDatabaseServiceTableChecker>();

            Assert.That(instance, Is.Not.Null);
        }


        [Test]
        public void IDatabaseServiceViewCheckerShouldBeRegister()
        {
            var instance = GetService<IDatabaseServiceViewChecker>();

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void IDatabaseServiceTriggerCheckerShouldBeRegister()
        {
            var instance = GetService<IDatabaseServiceTriggerChecker>();

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void IDatabaseServiceStoredProcedureCheckerShouldBeRegister()
        {
            var instance = GetService<IDatabaseServiceStoredProcedureChecker>();

            Assert.That(instance, Is.Not.Null);
        }
    }
}
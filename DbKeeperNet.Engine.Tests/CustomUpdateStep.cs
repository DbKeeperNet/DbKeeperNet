using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests
{
    public sealed class CustomUpdateStep: ICustomUpdateStep
    {
        public static bool Executed;

        public CustomUpdateStep()
        {
            Executed = false;
        }

        #region ICustomUpdateStep Members

        public void ExecuteUpdate(IUpdateContext context, CustomUpdateStepParamType[] param)
        {
            Assert.That(context.CurrentAssemblyName, Is.EqualTo("DbUpdater.Engine"));
            Assert.That(context.CurrentVersion, Is.EqualTo("1.00"));
            Assert.That(context.CurrentStep, Is.EqualTo(1));

            Executed = true;
        }

        #endregion
    }
}

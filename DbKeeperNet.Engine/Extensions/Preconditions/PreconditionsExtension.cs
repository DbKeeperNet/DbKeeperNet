using System;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    public sealed class PreconditionsExtension : IExtension
    {
        #region IExtension Members

        public void Initialize(IUpdateContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.RegisterPrecondition(new StepNotExecuted());
            context.RegisterPrecondition(new DbTableNotFound());
            context.RegisterPrecondition(new ForceExecution());
            context.RegisterPrecondition(new DbProcedureNotFound());
            context.RegisterPrecondition(new DbIndexNotFound());
            context.RegisterPrecondition(new DbForeignKeyNotFound());
            context.RegisterPrecondition(new DbViewNotFound());
            context.RegisterPrecondition(new DbType());
            context.RegisterPrecondition(new DbPrimaryKeyNotFound());
            context.RegisterPrecondition(new DbTriggerNotFound());
            context.RegisterPrecondition(new OraDbSequenceNotFound());
        }

        #endregion
    }
}

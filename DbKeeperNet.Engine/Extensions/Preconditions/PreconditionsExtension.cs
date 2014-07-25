using System;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Registration entry point for built-in preconditions
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>Registers support for following preconditions:</listheader>
    /// <item><see cref="DbForeignKeyNotFound">Foreign key does not exists</see></item>
    /// <item><see cref="DbIndexNotFound">Database index does not exists</see></item>
    /// <item><see cref="DbPrimaryKeyNotFound">Primary key does not exists</see></item>
    /// <item><see cref="DbProcedureNotFound">Stored procedure does not exists</see></item>
    /// <item><see cref="DbTableNotFound">Table does not exists</see></item>
    /// <item><see cref="DbType">Database is of expected type</see></item>
    /// <item><see cref="DbViewNotFound">View does not exists</see></item>
    /// <item><see cref="ForceExecution">Always execute</see></item>
    /// <item><see cref="OraDbSequenceNotFound">Oracle database sequence does not exists</see></item>
    /// <item><see cref="StepNotExecuted">Upgrade step with given identifiers wasn't yet executed</see></item>
    /// <item><see cref="UserNotFound">User identified by login name does not exist</see></item>
    /// <item><see cref="RoleNotFound">Role identified by name does not exist</see></item>
    /// </list>
    /// 
    /// All providers implement interface <see cref="IPrecondition"/>
    /// </remarks>
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
            context.RegisterPrecondition(new UserNotFound());
            context.RegisterPrecondition(new RoleNotFound());
        }

        #endregion
    }
}

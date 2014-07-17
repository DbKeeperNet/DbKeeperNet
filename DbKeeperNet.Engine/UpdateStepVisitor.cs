using System;
using System.Globalization;
using DbKeeperNet.Engine.Resources;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Database update step visitor implementation
    /// </summary>
    /// <remarks>This implementation is directly responsible for execution
    /// of each step</remarks>
    public class UpdateStepVisitor : IUpdateStepVisitor
    {
        #region Private fields

        private readonly IUpdateContext _context;
        private readonly ISqlScriptSplitter _scriptSplitter;

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">An instance of the update context</param>
        /// <param name="scriptSplitter">Associtated script splitter instance</param>
        public UpdateStepVisitor(IUpdateContext context, ISqlScriptSplitter scriptSplitter)
        {
            _context = context;
            _scriptSplitter = scriptSplitter;
        }

        /// <summary>
        /// Process upgrade step of type <see cref="AspNetAccountCreateUpdateStepType"/>
        /// </summary>
        /// <param name="step">Step parameters</param>
        public void Visit(AspNetAccountCreateUpdateStepType step)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Process upgrade step of type <see cref="UpdateDbStepType"/>
        /// </summary>
        /// <param name="step">Step parameters</param>
        public void Visit(UpdateDbStepType step)
        {
            UpdateDbAlternativeStatementType usableStatement = null;
            UpdateDbAlternativeStatementType commonStatement = null;

            foreach (UpdateDbAlternativeStatementType statement in step.AlternativeStatement)
            {
                if (statement.DbType.Equals(@"all", StringComparison.Ordinal))
                    commonStatement = statement;

                if (_context.DatabaseService.IsDbType(statement.DbType))
                {
                    usableStatement = statement;
                    break;
                }
            }

            if (usableStatement == null)
                usableStatement = commonStatement;

            if (usableStatement != null)
            {
                var stepCount = 0;

                foreach (var statement in _scriptSplitter.SplitScript(usableStatement.Value))
                {
                    _context.Logger.TraceInformation(UpdaterMessages.ExecutingCommandPart, ++stepCount);
                    _context.DatabaseService.ExecuteSql(statement);
                    _context.Logger.TraceInformation(UpdaterMessages.FinishedCommandPart, stepCount);
                }
            }
            else
            {
                _context.Logger.TraceWarning(UpdaterMessages.AlternativeSqlStatementNotFound, _context.DatabaseService.Name);
            }
        }

        /// <summary>
        /// Process upgrade step of type <see cref="CustomUpdateStepType"/>
        /// </summary>
        /// <param name="step">Step parameters</param>
        public void Visit(CustomUpdateStepType step)
        {
            Type type = Type.GetType(step.Type);

            if (type == null)
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, UpdaterMessages.CustomStepTypeNotFound, step.Type));

            ICustomUpdateStep customStep = (ICustomUpdateStep)Activator.CreateInstance(type);
            customStep.ExecuteUpdate(_context, step.Param);
        }
    }
}
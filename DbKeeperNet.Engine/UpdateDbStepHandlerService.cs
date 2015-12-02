using System;
using DbKeeperNet.Engine.Resources;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Handler of <see cref="UpdateDbStepType"/>
    /// </summary>
    public class UpdateDbStepHandlerService : UpdateStepHandlerService<UpdateDbStepType>
    {
        #region 

        private ISqlScriptSplitter _scriptSplitter;

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scriptSplitter">Sql script splitting strategy</param>
        public UpdateDbStepHandlerService(ISqlScriptSplitter scriptSplitter)
        {
            _scriptSplitter = scriptSplitter;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        protected override void Handle(UpdateDbStepType castedStep, IUpdateContext context)
        {
            UpdateDbAlternativeStatementType usableStatement = null;
            UpdateDbAlternativeStatementType commonStatement = null;

            foreach (var statement in castedStep.AlternativeStatement)
            {
                if (statement.DbType.Equals(@"all", StringComparison.Ordinal))
                    commonStatement = statement;

                if (context.DatabaseService.IsDbType(statement.DbType))
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
                    context.Logger.TraceInformation(UpdateStepVisitorMessages.ExecutingCommandPart, ++stepCount);
                    context.DatabaseService.ExecuteSql(statement);
                    context.Logger.TraceInformation(UpdateStepVisitorMessages.FinishedCommandPart, stepCount);
                }
            }
            else
            {
                context.Logger.TraceWarning(UpdateStepVisitorMessages.AlternativeSqlStatementNotFound, context.DatabaseService.Name);
            }
        }
    }
}
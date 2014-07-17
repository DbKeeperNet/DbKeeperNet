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
        private readonly IAspNetMembershipAdapter _aspNetMembershipAdapter;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">An instance of the update context</param>
        /// <param name="scriptSplitter">Associtated script splitter instance</param>
        /// <param name="aspNetMembershipAdapter">ASP.NET member ship adapter</param>
        public UpdateStepVisitor(IUpdateContext context, ISqlScriptSplitter scriptSplitter, IAspNetMembershipAdapter aspNetMembershipAdapter)
        {
            _context = context;
            _scriptSplitter = scriptSplitter;
            _aspNetMembershipAdapter = aspNetMembershipAdapter;
        }

        /// <summary>
        /// Process upgrade step of type <see cref="AspNetAccountCreateUpdateStepType"/>
        /// </summary>
        /// <param name="step">Step parameters</param>
        public void Visit(AspNetAccountCreateUpdateStepType step)
        {
            _context.Logger.TraceInformation("Going to use adapter {0}", _aspNetMembershipAdapter);

            _context.Logger.TraceInformation("Adding user {0}", step.UserName);
            _aspNetMembershipAdapter.CreateUser(step.UserName, step.Password, step.Mail);
            _context.Logger.TraceInformation("Added user {0}", step.UserName);

            if (step.Role != null && step.Role.Length != 0)
            {
                _context.Logger.TraceInformation("Adding user {0} to roles {1}", step.UserName, string.Join(@",", step.Role));
                _aspNetMembershipAdapter.AddUserToRoles(step.UserName, step.Role);
                _context.Logger.TraceInformation("User {0} added to requested roles", step.UserName);
            }
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
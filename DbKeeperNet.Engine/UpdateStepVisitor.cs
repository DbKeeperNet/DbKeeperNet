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
        /// <param name="updateStep">Step parameters</param>
        public void Visit(AspNetAccountCreateUpdateStepType updateStep)
        {
            if (updateStep == null) throw new ArgumentNullException(@"updateStep");

            _context.Logger.TraceInformation(UpdateStepVisitorMessages.GoingToUseAdapter, _aspNetMembershipAdapter);

            _context.Logger.TraceInformation(UpdateStepVisitorMessages.AddingUser, updateStep.UserName);
            _aspNetMembershipAdapter.CreateUser(updateStep.UserName, updateStep.Password, updateStep.Mail);
            _context.Logger.TraceInformation(UpdateStepVisitorMessages.AddedUser, updateStep.UserName);

            if (updateStep.Role != null && updateStep.Role.Length != 0)
            {
                _context.Logger.TraceInformation(UpdateStepVisitorMessages.AddingUserToRoles, updateStep.UserName, string.Join(@",", updateStep.Role));
                _aspNetMembershipAdapter.AddUserToRoles(updateStep.UserName, updateStep.Role);
                _context.Logger.TraceInformation(UpdateStepVisitorMessages.UserAddedToRoles, updateStep.UserName);
            }
        }

        /// <summary>
        /// Process upgrade step of type <see cref="AspNetRoleCreateUpdateStepType"/>
        /// </summary>
        /// <param name="updateStep">Step parameters</param>
        public void Visit(AspNetRoleCreateUpdateStepType updateStep)
        {
            if (updateStep == null) throw new ArgumentNullException(@"updateStep");

            _context.Logger.TraceInformation(UpdateStepVisitorMessages.GoingToUseAdapter, _aspNetMembershipAdapter);
            _context.Logger.TraceInformation(UpdateStepVisitorMessages.AddingRole, updateStep.RoleName);
            _aspNetMembershipAdapter.CreateRole(updateStep.RoleName);
            _context.Logger.TraceInformation(UpdateStepVisitorMessages.AddedRole, updateStep.RoleName);
        }

        /// <summary>
        /// Process upgrade step of type <see cref="AspNetRoleDeleteUpdateStepType"/>
        /// </summary>
        /// <param name="updateStep">Step parameters</param>
        public void Visit(AspNetRoleDeleteUpdateStepType updateStep)
        {
            if (updateStep == null) throw new ArgumentNullException(@"updateStep");

            _context.Logger.TraceInformation(UpdateStepVisitorMessages.GoingToUseAdapter, _aspNetMembershipAdapter);
            _context.Logger.TraceInformation(UpdateStepVisitorMessages.DeletingRole, updateStep.RoleName);
            _aspNetMembershipAdapter.DeleteRole(updateStep.RoleName);
            _context.Logger.TraceInformation(UpdateStepVisitorMessages.DeletedRole, updateStep.RoleName);
        }

        /// <summary>
        /// Process upgrade step of type <see cref="AspNetAccountDeleteUpdateStepType"/>
        /// </summary>
        /// <param name="updateStep">Step parameters</param>
        public void Visit(AspNetAccountDeleteUpdateStepType updateStep)
        {
            if (updateStep == null) throw new ArgumentNullException(@"updateStep");

            _context.Logger.TraceInformation(UpdateStepVisitorMessages.GoingToUseAdapter, _aspNetMembershipAdapter);
            _context.Logger.TraceInformation(UpdateStepVisitorMessages.DeletingUser, updateStep.UserName);

            if (_aspNetMembershipAdapter.DeleteUser(updateStep.UserName))
            {
                _context.Logger.TraceInformation(UpdateStepVisitorMessages.DeletedUser, updateStep.UserName);
            }
            else
            {
                _context.Logger.TraceWarning(UpdateStepVisitorMessages.UserNotDeleted, updateStep.UserName);
            }
        }

        /// <summary>
        /// Process upgrade step of type <see cref="UpdateDbStepType"/>
        /// </summary>
        /// <param name="updateStep">Step parameters</param>
        public void Visit(UpdateDbStepType updateStep)
        {
            if (updateStep == null) throw new ArgumentNullException(@"updateStep");

            UpdateDbAlternativeStatementType usableStatement = null;
            UpdateDbAlternativeStatementType commonStatement = null;

            foreach (UpdateDbAlternativeStatementType statement in updateStep.AlternativeStatement)
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
                    _context.Logger.TraceInformation(UpdateStepVisitorMessages.ExecutingCommandPart, ++stepCount);
                    _context.DatabaseService.ExecuteSql(statement);
                    _context.Logger.TraceInformation(UpdateStepVisitorMessages.FinishedCommandPart, stepCount);
                }
            }
            else
            {
                _context.Logger.TraceWarning(UpdateStepVisitorMessages.AlternativeSqlStatementNotFound, _context.DatabaseService.Name);
            }
        }

        /// <summary>
        /// Process upgrade step of type <see cref="CustomUpdateStepType"/>
        /// </summary>
        /// <param name="updateStep">Step parameters</param>
        public void Visit(CustomUpdateStepType updateStep)
        {
            if (updateStep == null) throw new ArgumentNullException(@"updateStep");

            Type type = Type.GetType(updateStep.Type);

            if (type == null)
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, UpdateStepVisitorMessages.CustomStepTypeNotFound, updateStep.Type));

            ICustomUpdateStep customStep = (ICustomUpdateStep)Activator.CreateInstance(type);
            customStep.ExecuteUpdate(_context, updateStep.Param);
        }
    }
}
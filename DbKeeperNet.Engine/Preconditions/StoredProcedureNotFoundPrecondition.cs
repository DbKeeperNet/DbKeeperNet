using System;

namespace DbKeeperNet.Engine.Preconditions
{
    /// <summary>
    /// Condition verifies that procedure with given name doesn't exist.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbProcedureNotFound</c>.
    /// It has one parameter which should contain tested database
    /// procedure name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Procedure testing_proc not found" Precondition="DbProcedureNotFound">
    ///   <Param>testing_proc</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public class StoredProcedureNotFoundPrecondition : IPreconditionHandler
    {
        private readonly IDatabaseServiceStoredProcedureChecker _checker;

        public StoredProcedureNotFoundPrecondition(IDatabaseServiceStoredProcedureChecker checker)
        {
            _checker = checker;
        }

        public bool CanHandle(UpdateStepContextPrecondition context)
        {
            return context.Precondition.Precondition == @"DbProcedureNotFound";
        }

        public bool IsMet(UpdateStepContextPrecondition context)
        {
            var precondition = context.Precondition;
            var param = precondition.Param;

            if ((param == null) || (param.Length == 0) || (String.IsNullOrEmpty(param[0].Value)))
                throw new ArgumentNullException(nameof(precondition));

            var result = !_checker.Exists(param[0].Value);

            return result;
        }
    }
}
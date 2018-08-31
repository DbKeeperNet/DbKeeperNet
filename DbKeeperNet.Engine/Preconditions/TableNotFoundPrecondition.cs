using System;

namespace DbKeeperNet.Engine.Preconditions
{
    /// <summary>
    /// Condition verifies that table with given name doesn't exist.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbTableNotFound</c>.
    /// It has one parameter which should contain tested database
    /// table name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Table testing_table not found" Precondition="DbTableNotFound">
    ///   <Param>testing_table</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public class TableNotFoundPrecondition : IPreconditionHandler
    {
        private readonly IDatabaseServiceTableChecker _checker;

        public TableNotFoundPrecondition(IDatabaseServiceTableChecker checker)
        {
            _checker = checker;
        }

        public bool CanHandle(UpdateStepContextPrecondition context)
        {
            return context.Precondition.Precondition == @"DbTableNotFound";
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
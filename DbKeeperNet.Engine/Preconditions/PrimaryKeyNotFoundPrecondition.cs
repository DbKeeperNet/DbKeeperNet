using System;

namespace DbKeeperNet.Engine.Preconditions
{
    /// <summary>
    /// Condition verifies that primary key with specified
    /// name and table doesn't exist in database.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbPrimaryKeyNotFound</c>.
    /// It has two parameters which should contain tested database
    /// primary key name and table name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Primary key PK_test not found" Precondition="DbPrimaryKeyNotFound">
    ///   <Param>PK_test</Param>
    ///   <Param>table_test_pk</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public class PrimaryKeyNotFoundPrecondition : IPreconditionHandler
    {
        private readonly IDatabaseServicePrimaryKeyChecker _checker;

        public PrimaryKeyNotFoundPrecondition(IDatabaseServicePrimaryKeyChecker checker)
        {
            _checker = checker;
        }

        public bool CanHandle(UpdateStepContextPrecondition context)
        {
            return context.Precondition.Precondition == @"DbPrimaryKeyNotFound";
        }

        public bool IsMet(UpdateStepContextPrecondition context)
        {
            var precondition = context.Precondition;
            var param = precondition.Param;

            if ((precondition == null) || (param == null) || (param.Length != 2) || (String.IsNullOrEmpty(param[0].Value)) || (String.IsNullOrEmpty(param[1].Value)))
                throw new ArgumentNullException(@"precondition");

            var result = !_checker.Exists(param[0].Value, param[1].Value);

            return result;
        }
    }
}
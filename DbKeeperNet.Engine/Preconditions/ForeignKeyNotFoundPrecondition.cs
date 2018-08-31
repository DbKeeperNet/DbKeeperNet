using System;

namespace DbKeeperNet.Engine.Preconditions
{
    /// <summary>
    /// Condition verifies that foreign key with specified
    /// name doesn't exist for table in database.
    /// </summary>
    /// <remarks>
    /// <para>Condition reference name is <c>DbForeignKeyNotFound</c>.
    /// It has two parameters which should contain tested database
    /// foreign key name and table name.
    /// </para>
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Foreign key FK_test not found" Precondition="DbForeignKeyNotFound">
    ///   <Param>FK_test</Param>
    ///   <Param>table_test_fk</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public class ForeignKeyNotFoundPrecondition : IPreconditionHandler
    {
        private readonly IDatabaseServiceForeignKeyChecker _checker;

        public ForeignKeyNotFoundPrecondition(IDatabaseServiceForeignKeyChecker checker)
        {
            _checker = checker;
        }

        public bool CanHandle(UpdateStepContextPrecondition context)
        {
            return context.Precondition.Precondition == @"DbForeignKeyNotFound";
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
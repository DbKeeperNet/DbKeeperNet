using System;

namespace DbKeeperNet.Engine.Preconditions
{
    /// <summary>
    /// Condition verifies that index with given name for given table doesn't exist.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbIndexNotFound</c>.
    /// It has two parameters which should contain tested database
    /// index name and table name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Index UQ_test not found" Precondition="DbIndexNotFound">
    ///   <Param>UQ_test</Param>
    ///   <Param>table_test_index</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public class IndexNotFoundPrecondition : IPreconditionHandler
    {
        private readonly IDatabaseServiceIndexChecker _checker;

        public IndexNotFoundPrecondition(IDatabaseServiceIndexChecker checker)
        {
            _checker = checker;
        }

        public bool CanHandle(UpdateStepContextPrecondition context)
        {
            return context.Precondition.Precondition == @"DbIndexNotFound";
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
using System;

namespace DbKeeperNet.Engine.Preconditions
{
    /// <summary>
    /// Condition verifies that view with given name doesn't exist.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbViewNotFound</c>.
    /// It has one parameter which should contain tested database
    /// view name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="View testing_view not found" Precondition="DbViewNotFound">
    ///   <Param>testing_view</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public class ViewNotFoundPrecondition : IPreconditionHandler
    {
        private readonly IDatabaseServiceViewChecker _checker;

        public ViewNotFoundPrecondition(IDatabaseServiceViewChecker checker)
        {
            _checker = checker;
        }

        public bool CanHandle(UpdateStepContextPrecondition context)
        {
            return context.Precondition.Precondition == @"DbViewNotFound";
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
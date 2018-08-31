using System;

namespace DbKeeperNet.Engine.Preconditions
{
    /// <summary>
    /// Condition verifies that trigger with specified
    /// name and doesn't exist in database.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbTriggerNotFound</c>.
    /// It has one parameter which should contain tested database
    /// trigger name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Trigger TR_Update_Primary not found" Precondition="DbTriggerNotFound">
    ///   <Param>TR_Update_Primary</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public class TriggerNotFoundPrecondition : IPreconditionHandler
    {
        private readonly IDatabaseServiceTriggerChecker _checker;

        public TriggerNotFoundPrecondition(IDatabaseServiceTriggerChecker checker)
        {
            _checker = checker;
        }

        public bool CanHandle(UpdateStepContextPrecondition context)
        {
            return context.Precondition.Precondition == @"DbTriggerNotFound";
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
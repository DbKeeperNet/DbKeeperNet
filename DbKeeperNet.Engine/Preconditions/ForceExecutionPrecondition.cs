namespace DbKeeperNet.Engine.Preconditions
{
    /// <summary>
    /// Condition forces script to be always executed.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>ForceExecution</c>.
    /// It has no parameters.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="View testing_view not found" Precondition="ForceExecution" />
    /// ]]>
    /// </code>
    /// </example>
    public class ForceExecutionPrecondition : IPreconditionHandler
    {
        public bool CanHandle(UpdateStepContextPrecondition precondition)
        {
            return precondition.Precondition.Precondition == @"ForceExecution";
        }

        public bool IsMet(UpdateStepContextPrecondition precondition)
        {
            return true;
        }
    }
}
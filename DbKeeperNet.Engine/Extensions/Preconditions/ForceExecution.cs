namespace DbKeeperNet.Engine.Extensions.Preconditions
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
    public sealed class ForceExecution : IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"ForceExecution"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            return true;
        }

        #endregion
    }
}

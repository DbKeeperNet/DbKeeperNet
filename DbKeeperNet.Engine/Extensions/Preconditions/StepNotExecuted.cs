using System;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that this update step, uniquely identified
    /// by current @Assembly, @Version and @Step wasn't already executed.
    /// </summary>
    /// <remarks>
    /// Each database upgrade step (if not explicitely disabled) is marked
    /// as executed based on unique identifier above. The way, how this information
    /// is stored depeneds on current database service.
    /// 
    /// Condition reference name is <c>StepNotExecuted</c>.
    /// It has no additional parameters.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Step not executed" Precondition="StepNotExecuted" />
    /// ]]>
    /// </code>
    /// </example>
    public sealed class StepNotExecuted : IPrecondition
    {
        #region IPrecondition Members

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");

            bool result = !context.DatabaseService.IsUpdateStepExecuted(context.CurrentAssemblyName, context.CurrentVersion, context.CurrentStep);

            return result;
        }

        public string Name
        {
            get { return @"StepNotExecuted"; }
        }

        #endregion
    }
}

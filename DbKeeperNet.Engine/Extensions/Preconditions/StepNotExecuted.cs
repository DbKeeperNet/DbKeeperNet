using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that this update step, uniquely identified
    /// by current @Assembly, @Version and @Step wasn't already executed.
    /// 
    /// Each database upgrade step (if not explicitely disabled) is marked
    /// as executed based on unique identifier above. The way, how this information
    /// is stored depeneds on current database service.
    /// 
    /// Condition reference name is <value>StepNotExecuted</value>.
    /// It has no additional parameters.
    /// <example>
    /// <![CDATA[
    /// <Precondition FriendlyName="Step not executed" Precondition="StepNotExecuted" />
    /// ]]>
    /// </example>
    /// </summary>
    class StepNotExecuted: IPrecondition
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

using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    class StepNotExecuted: IPrecondition
    {
        #region IPrecondition Members

        public bool CheckPrecondition(IUpdateContext context, string[] param)
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

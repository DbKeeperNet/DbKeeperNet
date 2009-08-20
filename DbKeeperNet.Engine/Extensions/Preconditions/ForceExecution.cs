using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    class ForceExecution: IPrecondition
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

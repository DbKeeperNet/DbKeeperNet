using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine;

namespace DbKeeperNet.ComplexDemo
{
    public class ComplexDemoCustomStep: ICustomUpdateStep
    {
        #region ICustomUpdateStep Members

        public void ExecuteUpdate(IUpdateContext context, CustomUpdateStepParamType[] param)
        {
            context.Logger.TraceInformation("Executing ComplexDemoCustomStep, p[0] = {0}", param[0].Value);
        }

        #endregion
    }
}

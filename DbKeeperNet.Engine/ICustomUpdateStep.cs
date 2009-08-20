using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Defines contract how to implement custom installation
    /// step.
    /// </summary>
    public interface ICustomUpdateStep
    {
        /// <summary>
        /// Method executed as an action during installation.
        /// </summary>
        /// <param name="context">Current update context</param>
        /// <param name="param">Optional parameters (with optional name) which can be passed thru the installation XML</param>
        void ExecuteUpdate(IUpdateContext context, CustomUpdateStepParamType[] param);
    }
}

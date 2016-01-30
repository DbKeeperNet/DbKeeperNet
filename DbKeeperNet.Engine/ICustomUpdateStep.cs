namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Defines contract how to implement custom installation
    /// step.
    /// </summary>
    /// <remarks>
    /// Custom update steps are the way, how to implement missing
    /// functionality such as in-code data transformation etc.
    /// 
    /// Active database connection and all services (such as logging)
    /// are available through the passed context.
    /// </remarks>
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

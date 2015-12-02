namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Precondition definition contract.
    /// </summary>
    public interface IPrecondition
    {
        /// <summary>
        /// Precondition name as it's available and used in
        /// installation XML document.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Method called for precondition evaluation. All specified
        /// preconditions must be met (return true) when checking
        /// whether step should or should not be executed.
        /// </summary>
        /// <param name="context">Current update context</param>
        /// <param name="param">Optional parameters which can be passed through
        /// installation XML document. Each parameter can be optionally named.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item><c>true</c> - condition was met, step can be executed.</item>
        /// <item><c>false</c> - prevent step from execution.</item>
        /// </list>
        /// </returns>
        bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param);
    }
}

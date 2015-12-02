using System;

namespace DbKeeperNet.Engine.Windows.Extensions.Preconditions
{
    /// <summary>
    /// Registration entry point for built-in preconditions
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>Registers support for following preconditions:</listheader>
    /// <item><see cref="OraDbSequenceNotFound">Oracle database sequence not found</see></item>
    /// </list>
    /// 
    /// All providers implement interface <see cref="IPrecondition"/>
    /// </remarks>
    public sealed class PreconditionsExtension : IExtension
    {
        #region IExtension Members

        public void Initialize(IUpdateContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.RegisterPrecondition(new OraDbSequenceNotFound());
        }

        #endregion
    }
}

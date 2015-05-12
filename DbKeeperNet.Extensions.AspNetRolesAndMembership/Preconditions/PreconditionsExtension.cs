using System;
using System.Data;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.AspNetRolesAndMembership.Preconditions
{
    /// <summary>
    /// Registration entry point for built-in preconditions
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>Registers support for following preconditions:</listheader>
    /// <item><see cref="UserNotFound">User identified by login name does not exist</see></item>
    /// <item><see cref="RoleNotFound">Role identified by name does not exist</see></item>
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

            context.RegisterPrecondition(new UserNotFound());
            context.RegisterPrecondition(new RoleNotFound());
        }

        #endregion
    }
}

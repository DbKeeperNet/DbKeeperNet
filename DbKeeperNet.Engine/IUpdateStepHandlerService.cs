using System;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Service handling execution of the <see cref="HandledType"/>
    /// XML element.
    /// </summary>
    public interface IUpdateStepHandlerService
    {
        /// <summary>
        /// Type of .NET class representing the given XML element
        /// </summary>
        Type HandledType { get; }
        
        /// <summary>
        /// Method called to handle the given <see cref="HandledType"/>.
        /// </summary>
        /// <param name="updateStep">An instance of <see cref="HandledType"/></param>
        /// <param name="context">Update context providing necessary session data for upgrade execution</param>
        void Handle(UpdateStepBaseType updateStep, IUpdateContext context);
    }
}
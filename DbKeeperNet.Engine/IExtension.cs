namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Extension bootstrap and marking interface.
    /// </summary>
    /// <remarks>
    /// When an extension assembly is loaded, this interfaces
    /// allows registration of all provided services.
    /// 
    /// For examples look on built-in services.
    /// </remarks>
    public interface IExtension
    {
        /// <summary>
        /// Method invoked for extension initialization. Register
        /// all provided services and new preconditions here.
        /// </summary>
        /// <param name="context">Current update context</param>
        /// <see cref="IUpdateContext.RegisterPrecondition"/>
        /// <see cref="IUpdateContext.RegisterDatabaseService"/>
        /// <see cref="IUpdateContext.RegisterLoggingService"/>
        /// <see cref="IUpdateContext.RegisterUpdateStepHandler"/>
        /// <see cref="IUpdateContext.RegisterSchema"/>
        void Initialize(IUpdateContext context);
    }
}

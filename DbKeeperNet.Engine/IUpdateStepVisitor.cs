namespace DbKeeperNet.Engine
{
    /// <summary>
    /// An interface being able process any database upgrade step
    /// </summary>
    /// <see cref="UpdateDbStepType"/>
    /// <see cref="AspNetAccountCreateUpdateStepType"/>
    /// <see cref="AspNetAccountDeleteUpdateStepType"/>
    /// <see cref="AspNetRoleCreateUpdateStepType"/>
    /// <see cref="AspNetRoleDeleteUpdateStepType"/>
    /// <see cref="CustomUpdateStepType"/>
    /// <see cref="UpdateStepBaseType"/>
    public interface IUpdateStepVisitor
    {
        /// <summary>
        /// Process upgrade step of type <see cref="AspNetAccountCreateUpdateStepType"/>
        /// </summary>
        /// <param name="updateStep">Step parameters</param>
        void Visit(AspNetAccountCreateUpdateStepType updateStep);

        /// <summary>
        /// Process upgrade step of type <see cref="AspNetAccountDeleteUpdateStepType"/>
        /// </summary>
        /// <param name="updateStep">Step parameters</param>
        void Visit(AspNetAccountDeleteUpdateStepType updateStep);

        /// <summary>
        /// Process upgrade step of type <see cref="UpdateDbStepType"/>
        /// </summary>
        /// <param name="updateStep">Step parameters</param>
        void Visit(UpdateDbStepType updateStep);

        /// <summary>
        /// Process upgrade step of type <see cref="CustomUpdateStepType"/>
        /// </summary>
        /// <param name="updateStep">Step parameters</param>
        void Visit(CustomUpdateStepType updateStep);

        /// <summary>
        /// Process upgrade step of type <see cref="AspNetRoleCreateUpdateStepType"/>
        /// </summary>
        /// <param name="updateStep">Step parameters</param>
        void Visit(AspNetRoleCreateUpdateStepType updateStep);

        /// <summary>
        /// Process upgrade step of type <see cref="AspNetRoleDeleteUpdateStepType"/>
        /// </summary>
        /// <param name="updateStep">Step parameters</param>
        void Visit(AspNetRoleDeleteUpdateStepType updateStep);
    }
}
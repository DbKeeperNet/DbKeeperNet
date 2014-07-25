namespace DbKeeperNet.Engine
{
    /// <summary>
    /// An interface being able process any database upgrade step
    /// </summary>
    /// <see cref="UpdateDbStepType"/>
    /// <see cref="AspNetAccountCreateUpdateStepType"/>
    /// <see cref="AspNetAccountDeleteUpdateStepType"/>
    /// <see cref="AspNetRoleCreateUpdateStepType"/>
    /// <see cref="CustomUpdateStepType"/>
    /// <see cref="UpdateStepBaseType"/>
    public interface IUpdateStepVisitor
    {
        /// <summary>
        /// Process upgrade step of type <see cref="AspNetAccountCreateUpdateStepType"/>
        /// </summary>
        /// <param name="step">Step parameters</param>
        void Visit(AspNetAccountCreateUpdateStepType step);

        /// <summary>
        /// Process upgrade step of type <see cref="AspNetAccountDeleteUpdateStepType"/>
        /// </summary>
        /// <param name="step">Step parameters</param>
        void Visit(AspNetAccountDeleteUpdateStepType step);

        /// <summary>
        /// Process upgrade step of type <see cref="UpdateDbStepType"/>
        /// </summary>
        /// <param name="step">Step parameters</param>
        void Visit(UpdateDbStepType step);

        /// <summary>
        /// Process upgrade step of type <see cref="CustomUpdateStepType"/>
        /// </summary>
        /// <param name="step">Step parameters</param>
        void Visit(CustomUpdateStepType step);

        /// <summary>
        /// Process upgrade step of type <see cref="AspNetRoleCreateUpdateStepType"/>
        /// </summary>
        /// <param name="step">Step parameters</param>
        void Visit(AspNetRoleCreateUpdateStepType step);
    }
}
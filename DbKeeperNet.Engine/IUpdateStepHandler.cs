namespace DbKeeperNet.Engine
{
    public interface IUpdateStepHandler
    {
        bool CanHandle(UpdateStepContextWithPreconditions context);

        /// <summary>
        /// Execute the given update step
        /// </summary>
        /// <param name="context">Update context</param>
        void Execute(UpdateStepContextWithPreconditions context);
    }
}
namespace DbKeeperNet.Engine
{
    public class UpdateStepContext : UpdateContext
    {
        public UpdateStepContext(UpdateStepBaseType step, UpdateContext updateContext)
            : base(updateContext)
        {
            Step = step;
        }

        protected UpdateStepContext(UpdateStepContext context) : base(context)
        {
            Step = context.Step;
        }

        public UpdateStepBaseType Step { get; }

        public int StepNumber
        {
            get { return Step.Id; }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Step.FriendlyName))
            {
                return $"Update step[{AssemblyName},version={UpdateVersion},step={StepNumber}]";
            }

            return $"Update step '{Step.FriendlyName}' [{AssemblyName},version={UpdateVersion},step={StepNumber}]";
        }
    }
}
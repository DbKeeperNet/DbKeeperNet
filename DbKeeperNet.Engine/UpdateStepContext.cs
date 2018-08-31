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
            return $"Updates[{AssemblyName},version={UpdateVersion},step={StepNumber}]";
        }
    }
}
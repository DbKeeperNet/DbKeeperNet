namespace DbKeeperNet.Engine
{
    public class UpdateStepContextPrecondition : UpdateStepContextWithPreconditions
    {
        public UpdateStepContextPrecondition(UpdateStepContextWithPreconditions context, PreconditionType precondition) : base(context)
        {
            Precondition = precondition;
        }

        public UpdateStepContextPrecondition(UpdateStepContextPrecondition context) : base(context)
        {
            Precondition = context.Precondition;
        }

        public PreconditionType Precondition { get; }
    }
}
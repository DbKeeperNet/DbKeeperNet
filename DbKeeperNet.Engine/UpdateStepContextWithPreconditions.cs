using System.Collections.Generic;

namespace DbKeeperNet.Engine
{
    public class UpdateStepContextWithPreconditions<T> : UpdateStepContextWithPreconditions
        where T: UpdateStepBaseType
    {
        public UpdateStepContextWithPreconditions(UpdateStepContextWithPreconditions context) : base(context)
        {
        }

        public new T Step
        {
            get { return (T) base.Step; }
        }

        public bool IsTyped
        {
            get { return base.Step is T; }
        }
    }

    public class UpdateStepContextWithPreconditions : UpdateStepContext
    {
        public UpdateStepContextWithPreconditions(UpdateStepContext context, IEnumerable<PreconditionType> preconditions) : base(context)
        {
            Preconditions = preconditions;
        }

        protected UpdateStepContextWithPreconditions(UpdateStepContextWithPreconditions context) : base(context)
        {
            Preconditions = context.Preconditions;
        }

        public IEnumerable<PreconditionType> Preconditions { get; }
    }
}
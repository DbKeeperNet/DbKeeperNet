using System;

namespace DbKeeperNet.Engine.UpdateStepHandlers
{
    public abstract class UpdateStepHandlerBase<T> : IUpdateStepHandler
        where T: UpdateStepBaseType
    {
        public void Execute(UpdateStepContextWithPreconditions context)
        {
            if (!(context.Step is T))
            {
                throw new InvalidCastException();
            }

            Execute(new UpdateStepContextWithPreconditions<T>(context));
        }

        public bool CanHandle(UpdateStepContextWithPreconditions context)
        {
            return CanHandle(new UpdateStepContextWithPreconditions<T>(context));
        }

        protected abstract void Execute(UpdateStepContextWithPreconditions<T> context);

        protected virtual bool CanHandle(UpdateStepContextWithPreconditions<T> context)
        {
            return context.IsTyped;
        }
    }
}
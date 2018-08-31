using System;
using System.Collections.Generic;
using System.Linq;

namespace DbKeeperNet.Engine
{
    public class PreconditionService : IPreconditionService
    {
        private readonly IEnumerable<IPreconditionHandler> _preconditionHandlers;
        
        public PreconditionService(IEnumerable<IPreconditionHandler> preconditionHandlers)
        {
            _preconditionHandlers = preconditionHandlers;
        }

        public bool IsMet(UpdateStepContextWithPreconditions context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Preconditions == null) throw new InvalidOperationException();

            // TODO: context?
            return context.Preconditions.All(p => IsMet(new UpdateStepContextPrecondition(context, p)));
        }

        private bool IsMet(UpdateStepContextPrecondition precondition)
        {
            foreach (var handler in _preconditionHandlers)
            {
                if (!handler.CanHandle(precondition))
                {
                    continue;
                }
                
                return handler.IsMet(precondition);
            }

            return true;
            // TODO: fix
            //  throw new InvalidOperationException($"Precondition {precondition} not supported");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

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

            return context.Preconditions.All(p =>
            {
                var updateStepContextPrecondition = new UpdateStepContextPrecondition(context, p);
                var result = IsMet(updateStepContextPrecondition);
                return result;
            });
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
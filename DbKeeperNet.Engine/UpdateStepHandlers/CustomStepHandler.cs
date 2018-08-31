using System;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Engine.UpdateStepHandlers
{
    public class CustomStepHandler : UpdateStepHandlerBase<CustomUpdateStepType>
    {
        private readonly ILogger<CustomStepHandler> _logger;

        public CustomStepHandler(ILogger<CustomStepHandler> logger)
        {
            _logger = logger;
        }

        protected override void Execute(UpdateStepContextWithPreconditions<CustomUpdateStepType> context)
        {
            var castedStep = context.Step;

            var type = Type.GetType(castedStep.Type);

            if (type == null)
            {
                throw new ArgumentException($"Custom step {context} type '{castedStep.Type}' not found");
            }

            _logger.LogInformation("Going to create instance of {0}", type);

            var customStep = (ICustomUpdateStep)Activator.CreateInstance(type);

            _logger.LogInformation("Instance of {0} created [{1}] - going to execute step", type, customStep);

            try
            {
                customStep.ExecuteUpdate(context, castedStep.Param);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{context} failed with exception", ex);
            }

        }
    }
}
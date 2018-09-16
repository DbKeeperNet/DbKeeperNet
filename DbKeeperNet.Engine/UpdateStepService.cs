using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Engine
{
    public class UpdateStepService : IUpdateStepService
    {
        private readonly ILogger<UpdateStepService> _logger;
        private readonly IEnumerable<IUpdateStepHandler> _updateStepHandlers;
        private readonly IUpdateStepExecutedMarker _updateStepExecutedMarker;
        private readonly IDatabaseServiceTransactionProvider _transactionProvider;

        public UpdateStepService(
            ILogger<UpdateStepService> logger,
            IEnumerable<IUpdateStepHandler> updateStepHandlers, 
            IUpdateStepExecutedMarker updateStepExecutedMarker, 
            IDatabaseServiceTransactionProvider transactionProvider)
        {
            _logger = logger;
            _updateStepHandlers = updateStepHandlers;
            _updateStepExecutedMarker = updateStepExecutedMarker;
            _transactionProvider = transactionProvider;
        }

        public void Execute(UpdateStepContextWithPreconditions context)
        {
            foreach (var updateStepHandler in _updateStepHandlers)
            {
                if (!updateStepHandler.CanHandle(context))
                {
                    continue;
                }

                _logger.LogInformation($"Going to execute {context}");

                updateStepHandler.Execute(context);

                _logger.LogInformation($"{context} was executed");

                if (context.Step.MarkAsExecuted)
                {
                    try
                    {
                        _transactionProvider.BeginTransaction();

                        _updateStepExecutedMarker.MarkAsExecuted(context.AssemblyName, context.UpdateVersion,
                            context.StepNumber);

                        _logger.LogInformation($"{context} was marked as executed");

                        _transactionProvider.CommitTransaction();
                    }
                    catch(Exception ex)
                    {
                        _transactionProvider.RollbackTransaction();

                        throw new InvalidOperationException($"{context} couldn't be marked as executed", ex);
                    }
                }
            }
        }
    }
}
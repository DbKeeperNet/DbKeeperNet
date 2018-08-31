using System;
using System.Collections.Generic;

namespace DbKeeperNet.Engine
{
    public class UpdateStepService : IUpdateStepService
    {
        private readonly IEnumerable<IUpdateStepHandler> _updateStepHandlers;
        private readonly IUpdateStepExecutedMarker _updateStepExecutedMarker;
        private readonly IDatabaseServiceTransactionProvider _transactionProvider;

        public UpdateStepService(IEnumerable<IUpdateStepHandler> updateStepHandlers, IUpdateStepExecutedMarker updateStepExecutedMarker, IDatabaseServiceTransactionProvider transactionProvider)
        {
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

                updateStepHandler.Execute(context);

                if (context.Step.MarkAsExecuted)
                {
                    try
                    {
                        _transactionProvider.BeginTransaction();

                        _updateStepExecutedMarker.MarkAsExecuted(context.AssemblyName, context.UpdateVersion,
                            context.StepNumber);

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
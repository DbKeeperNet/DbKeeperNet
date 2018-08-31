using System;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Engine.UpdateStepHandlers
{
    public class DbCommandStepHandler : UpdateStepHandlerBase<UpdateDbStepType>
    {
        private readonly IDatabaseService _databaseService;
        private readonly IDatabaseServiceCommandHandler _commandHandler;
        private readonly IDatabaseServiceTransactionProvider _transactionProvider;
        private readonly ISqlScriptSplitter _scriptSplitter;
        private readonly ILogger<DbCommandStepHandler> _logger;

        public DbCommandStepHandler(
            IDatabaseService databaseService, 
            IDatabaseServiceCommandHandler commandHandler,
            IDatabaseServiceTransactionProvider transactionProvider,
            ISqlScriptSplitter scriptSplitter,
            ILogger<DbCommandStepHandler> logger)
        {
            _databaseService = databaseService;
            _commandHandler = commandHandler;
            _transactionProvider = transactionProvider;
            _scriptSplitter = scriptSplitter;
            _logger = logger;
        }

        protected override void Execute(UpdateStepContextWithPreconditions<UpdateDbStepType> context)
        {
            foreach (var statement in context.Step.AlternativeStatement)
            {
                _logger.LogInformation("Going to check whether statement for DbType={0} cannot be handled by this database", statement.DbType);

                if (!_databaseService.CanHandle(statement.DbType) && statement.DbType != "all")
                {
                    _logger.LogInformation("{0} for DbType={1} cannot be handled by this database", context, statement.DbType);

                    continue;
                }
                
                _transactionProvider.BeginTransaction();

                try
                {
                    foreach (var subStep in _scriptSplitter.SplitScript(statement.Value))
                    {
                        // TODO: add subcontext on script split?
                        _commandHandler.Execute(subStep);
                    }
                    _transactionProvider.CommitTransaction();
                }
                catch (Exception ex)
                {
                    // let's assume that the rollback is low chance to fail so we will not log the
                    // previous error first
                    _transactionProvider.RollbackTransaction();

                    throw new InvalidOperationException($"{context} failed with exception", ex);
                }
            }
        }
    }
}
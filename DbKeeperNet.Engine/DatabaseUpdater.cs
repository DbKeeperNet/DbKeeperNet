using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;

namespace DbKeeperNet.Engine
{
    public class DatabaseUpdater : IDatabaseUpdater
    {
        private readonly ILogger<DatabaseUpdater> _logger;
        private readonly IDatabaseServiceInstaller _databaseServiceInstaller;
        private readonly IUpdateScriptManager _updateScriptManager;
        private readonly IPreconditionService _preconditionService;
        private readonly IUpdateStepService _updateStepService;
        private readonly IScriptDeserializer _scriptDeserializer;
        
        public DatabaseUpdater(ILogger<DatabaseUpdater> logger,
            IDatabaseServiceInstaller databaseServiceInstaller,
            IUpdateScriptManager updateScriptManager,
            IPreconditionService preconditionService,
            IUpdateStepService updateStepService,
            IScriptDeserializer scriptDeserializer)
        {
            _logger = logger;
            _databaseServiceInstaller = databaseServiceInstaller;
            _updateScriptManager = updateScriptManager;
            _preconditionService = preconditionService;
            _updateStepService = updateStepService;
            _scriptDeserializer = scriptDeserializer;
        }

        public void ExecuteUpgrade()
        {
            try
            {
                SetupDatabase();

                foreach (var script in _updateScriptManager.Scripts)
                {
                    try
                    {
                        RunScript(script);
                    }
                    finally
                    {
                        script.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database upgrade failed");

                throw new DbKeeperNetException("Database upgrade failed with an exception", ex);
            }
        }

        private void SetupDatabase()
        {
            _logger.LogInformation("Going to setup the database using {0}", _databaseServiceInstaller);

            using (var installerScript = _databaseServiceInstaller.GetInstallerScript())
            {
                RunScript(installerScript);
            }

            _logger.LogInformation("Database setup finished");
        }



        private void RunScript(Stream scriptSource)
        {
            var script = _scriptDeserializer.GetDeserializedScript(scriptSource);

            var scriptContext = new ScriptContext(script);

            foreach (var update in script.Update)
            {
                var updateContext = new UpdateContext(update, scriptContext);

                foreach (var step in update.UpdateStep)
                {
                    var stepContext = new UpdateStepContext(step, updateContext);

                    var preconditions = ResolvePreconditions(stepContext, step, script);
                    var context = new UpdateStepContextWithPreconditions(stepContext, preconditions);
                    if (!_preconditionService.IsMet(context))
                    {
                        _logger.LogInformation($"{context} precondition resulted in skipping the step");
                        continue;
                    }

                
                    _updateStepService.Execute(context);
                }
            }
        }

        private IEnumerable<PreconditionType> ResolvePreconditions(UpdateStepContext stepContext, UpdateStepBaseType step,
            Updates script)
        {
            var preconditions = step.Preconditions;

            if (preconditions == null || preconditions.Length == 0)
            {
                _logger.LogInformation("{0} doesn't have precondition - using script defaults", stepContext);
                preconditions = script.DefaultPreconditions;
            }

            if (preconditions == null)
            {
                throw new InvalidOperationException($"Update script for {stepContext} doesn't have default preconditions specified");
            }

            return preconditions;
        }
    }
}
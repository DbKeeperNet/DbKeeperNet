using System.IO;
using DbKeeperNet.Engine;
using DbKeeperNet.Engine.UpdateStepHandlers;
using DbKeeperNet.Extensions.SqlServer;

namespace DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup
{
    public class MsSqlMembershipAndRolesSetupTypeHandlerService : UpdateStepHandlerBase<MsSqlMembershipAndRolesSetupType>
    {
        private readonly IDatabaseService _databaseService;

        public MsSqlMembershipAndRolesSetupTypeHandlerService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        protected override void Execute(UpdateStepContextWithPreconditions<MsSqlMembershipAndRolesSetupType> context)
        {
            using (var connection = _databaseService.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    using (var script = new StreamReader(typeof(MsSqlMembershipAndRolesSetupTypeHandlerService).Assembly.GetManifestResourceStream("DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup.MsSqlMembershipAndRolesSetup.sql")))
                    {
                        var scriptText = script.ReadToEnd();
                        var sqlScriptSplitter = new MsSqlScriptSplitter();

                        foreach (var scriptPart in sqlScriptSplitter.SplitScript(scriptText))
                        {
                            command.CommandText = scriptPart;
                            command.Transaction = null;
                            command.ExecuteNonQuery();
                        }
                    }
                }

            }
        }
    }
}
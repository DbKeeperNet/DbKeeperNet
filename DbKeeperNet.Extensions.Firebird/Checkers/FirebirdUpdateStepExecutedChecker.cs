using DbKeeperNet.Engine;
using FirebirdSql.Data.FirebirdClient;

namespace DbKeeperNet.Extensions.Firebird.Checkers
{
    public class FirebirdUpdateStepExecutedChecker : IUpdateStepExecutedChecker
    {
        private readonly IDatabaseService<FbConnection> _databaseService;
        
        public FirebirdUpdateStepExecutedChecker(IDatabaseService<FbConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool IsExecuted(string assembly, string version, int stepNumber)
        {
            bool result = false;
            
            using (var stepExecutedQuery = new FbCommand(
                @"select count(""version"")  from ""dbkeepernet_assembly"" asm
                    join ""dbkeepernet_version"" ver on asm.""id"" = ver.""dbkeepernet_assembly_id""
                    join ""dbkeepernet_step"" step on ver.""id"" = step.""dbkeepernet_version_id""
                    where step.""step"" = @step 
                    and ver.""version"" = @version and 
                    asm.""assembly"" = @assembly",
                _databaseService.GetOpenConnection()))
            {
                var assemblyParameter = new FbParameter("@assembly", FbDbType.Text) { Value = assembly };
                var versionParameter = new FbParameter("@version", FbDbType.Text) { Value = version };
                var stepParameter = new FbParameter("@step", FbDbType.Integer) { Value = stepNumber };

                stepExecutedQuery.Parameters.Add(assemblyParameter);
                stepExecutedQuery.Parameters.Add(versionParameter);
                stepExecutedQuery.Parameters.Add(stepParameter);

                var count = (int?)stepExecutedQuery.ExecuteScalar();

                if ((count.HasValue) && (count.Value > 0))
                    result = true;

                return result;
            }
        }


    }
}
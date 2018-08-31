using DbKeeperNet.Engine;
using MySql.Data.MySqlClient;

namespace DbKeeperNet.Extensions.Mysql.Checkers
{
    public class MySqlUpdateStepExecutedChecker : IUpdateStepExecutedChecker
    {
        private readonly IDatabaseService<MySqlConnection> _databaseService;

        public MySqlUpdateStepExecutedChecker(IDatabaseService<MySqlConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool IsExecuted(string assembly, string version, int stepNumber)
        {
            bool result = false;

            using (var stepExecutedQuery = new MySqlCommand(
                @"select count(version)  from dbkeepernet_assembly asm
                    join dbkeepernet_version ver on asm.id = ver.dbkeepernet_assembly_id
                    join dbkeepernet_step step on ver.id = step.dbkeepernet_version_id
		            where step.step = @step 
                    and ver.version = @version and 
                    asm.assembly = @assembly",
                _databaseService.GetOpenConnection()))
            {
                var assemblyParameter = new MySqlParameter("@assembly", MySqlDbType.Text) { Value = assembly };
                var versionParameter = new MySqlParameter("@version", MySqlDbType.Text) { Value = version };
                var stepParameter = new MySqlParameter("@step", MySqlDbType.Int32) { Value = stepNumber };

                stepExecutedQuery.Parameters.Add(assemblyParameter);
                stepExecutedQuery.Parameters.Add(versionParameter);
                stepExecutedQuery.Parameters.Add(stepParameter);

                var count = (long?)stepExecutedQuery.ExecuteScalar();

                if ((count.HasValue) && (count.Value > 0))
                    result = true;

                return result;
            }
        }


    }
}
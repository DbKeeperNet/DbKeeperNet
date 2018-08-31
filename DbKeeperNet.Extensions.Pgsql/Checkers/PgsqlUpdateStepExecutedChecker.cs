using DbKeeperNet.Engine;
using Npgsql;
using NpgsqlTypes;

namespace DbKeeperNet.Extensions.Pgsql.Checkers
{
    public class PgsqlUpdateStepExecutedChecker : IUpdateStepExecutedChecker
    {
        private readonly IDatabaseService<NpgsqlConnection> _databaseService;

        public PgsqlUpdateStepExecutedChecker(IDatabaseService<NpgsqlConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool IsExecuted(string assembly, string version, int stepNumber)
        {
            bool result = false;

            using (var stepExecutedQuery = new NpgsqlCommand(
                @"select count(version)  from dbkeepernet_assembly asm
                    join dbkeepernet_version ver on asm.id = ver.dbkeepernet_assembly_id
                    join dbkeepernet_step step on ver.id = step.dbkeepernet_version_id
                    where step.step = @step 
                    and ver.version = @version and 
                    asm.assembly = @assembly",
                _databaseService.GetOpenConnection()))
            {
                var assemblyParameter = new NpgsqlParameter("@assembly", NpgsqlDbType.Text) { Value = assembly };
                var versionParameter = new NpgsqlParameter("@version", NpgsqlDbType.Text) { Value = version };
                var stepParameter = new NpgsqlParameter("@step", NpgsqlDbType.Integer) { Value = stepNumber };

                stepExecutedQuery.Parameters.Add(assemblyParameter);
                stepExecutedQuery.Parameters.Add(versionParameter);
                stepExecutedQuery.Parameters.Add(stepParameter);

                long? count = (long?)stepExecutedQuery.ExecuteScalar();

                if ((count.HasValue) && (count.Value > 0))
                    result = true;

                return result;
            }
        }


    }
}
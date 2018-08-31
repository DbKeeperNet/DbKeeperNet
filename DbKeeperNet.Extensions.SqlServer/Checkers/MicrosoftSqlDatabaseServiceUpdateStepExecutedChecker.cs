using System;
using System.Data;
using System.Data.SqlClient;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.SqlServer.Checkers
{
    public class MicrosoftSqlDatabaseServiceUpdateStepExecutedChecker : IUpdateStepExecutedChecker
    {
        private readonly IDatabaseService<SqlConnection> _databaseService;

        public MicrosoftSqlDatabaseServiceUpdateStepExecutedChecker(IDatabaseService<SqlConnection> databaseService)
        {
            _databaseService = databaseService;
        }

        public bool IsExecuted(string assembly, string version, int stepNumber)
        {
            using (var cmd = _databaseService.GetOpenConnection().CreateCommand())
            {
                cmd.CommandText = "DbKeeperNetIsStepExecuted";
                cmd.CommandType = CommandType.StoredProcedure;

                var param = cmd.CreateParameter();
                param.ParameterName = "@assembly";
                param.Value = assembly;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@version";
                param.Value = version;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@step";
                param.Value = stepNumber;
                cmd.Parameters.Add(param);

                var result = (bool) cmd.ExecuteScalar();

                return result;
            }
        }
    }
}
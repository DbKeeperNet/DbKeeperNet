using System.Data;
using System.Data.SqlClient;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.SqlServer
{
    public class MicrosoftSqlDatabaseUpdateStepExecutedMarker : IUpdateStepExecutedMarker
    {
        private readonly IDatabaseService<SqlConnection> _connection;
        private readonly IDatabaseServiceTransactionProvider<SqlTransaction> _transactionProvider;

        public MicrosoftSqlDatabaseUpdateStepExecutedMarker(IDatabaseService<SqlConnection> connection, IDatabaseServiceTransactionProvider<SqlTransaction> transactionProvider)
        {
            _connection = connection;
            _transactionProvider = transactionProvider;
        }

        public void MarkAsExecuted(string assembly, string version, int stepNumber)
        {
            using (var cmd = _connection.GetOpenConnection().CreateCommand())
            {
                cmd.Transaction = _transactionProvider.GetTransaction();

                cmd.CommandText = "DbKeeperNetSetStepExecuted";
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

                cmd.ExecuteNonQuery();

            }
        }
    }
}
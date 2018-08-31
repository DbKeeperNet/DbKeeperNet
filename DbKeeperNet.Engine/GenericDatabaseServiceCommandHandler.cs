using System.Data;

namespace DbKeeperNet.Engine
{
    public class GenericDatabaseServiceCommandHandler : IDatabaseServiceCommandHandler
    {
        private readonly IDatabaseService _databaseService;
        private readonly IDatabaseServiceTransactionProvider _transactionProvider;

        public GenericDatabaseServiceCommandHandler(IDatabaseService databaseService, IDatabaseServiceTransactionProvider transactionProvider)
        {
            _databaseService = databaseService;
            _transactionProvider = transactionProvider;
        }

        public void Execute(string command)
        {
            using (var databaseCommand = _databaseService.GetOpenConnection().CreateCommand())
            {
                databaseCommand.Transaction = _transactionProvider.GetTransaction();
                databaseCommand.CommandText = command;
                databaseCommand.CommandType = CommandType.Text;
                databaseCommand.ExecuteNonQuery();
            }
        }
    }
}
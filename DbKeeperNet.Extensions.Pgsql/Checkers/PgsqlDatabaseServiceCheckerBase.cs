using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Pgsql.Checkers
{
    public class PgsqlDatabaseServiceCheckerBase
    {
        private readonly IDatabaseService _databaseService;

        public PgsqlDatabaseServiceCheckerBase(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        protected bool RetrieveSchemaInformationAndReturnTrueIfRowExists(string schemaCollectionName, string[] restrictions)
        {
            var schema = _databaseService.GetOpenConnection().GetSchema(schemaCollectionName, restrictions);

            return (schema.Rows.Count != 0);
        }
    }
}
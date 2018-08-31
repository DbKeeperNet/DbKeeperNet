using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Firebird.Checkers
{
    public abstract class FirebirdDatabaseServiceCheckerBase
    {
        private readonly IDatabaseService _databaseService;

        protected FirebirdDatabaseServiceCheckerBase(IDatabaseService databaseService)
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
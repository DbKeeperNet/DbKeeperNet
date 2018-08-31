using System;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Pgsql.Checkers
{
    public class PgsqlDatabaseServiceTableChecker : PgsqlDatabaseServiceCheckerBase, IDatabaseServiceTableChecker
    {
        public PgsqlDatabaseServiceTableChecker(IDatabaseService databaseService)
            : base(databaseService)
        {
        }

        public bool Exists(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string[] restrictions = new string[4];

            restrictions[2] = name;
            restrictions[3] = "BASE TABLE";

            return RetrieveSchemaInformationAndReturnTrueIfRowExists(@"Tables", restrictions);
        }
    }
}